import React, { useState, useEffect } from 'react';
import axios from 'axios';
import {
    CForm,
    CFormInput,
    CFormSelect,
    CButton,
    CCard,
    CCardBody,
    CCardHeader,
    CCol,
    CTable,
    CTableHeaderCell,
    CTableBody,
    CTableRow,
    CTableDataCell,
    CModal,
    CModalHeader,
    CModalBody,
    CModalFooter,
    CFormFeedback,

} from '@coreui/react';
import CIcon from '@coreui/icons-react';
import { cilTrash, cilPencil } from '@coreui/icons';

const DeleteRecordPage = () => {
    const [selectedDataType, setSelectedDataType] = useState('Electric');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [buildings, setBuildings] = useState([]);
    const [buildingId, setBuildingId] = useState('');
    const [records, setRecords] = useState([]);
    const [modalVisible, setModalVisible] = useState(false);
    const [editModalVisible, setEditModalVisible] = useState(false);
    const [recordToEdit, setRecordToEdit] = useState(null);
    const [recordToDelete, setRecordToDelete] = useState(null);
    const [validated, setValidated] = useState(false);
    const [validationErrors, setValidationErrors] = useState({});

    const baseUrl = 'http://localhost:5154/';
    const endpoint =
        selectedDataType === 'Electric'
            ? 'api/Electric'
            : selectedDataType === 'Water'
                ? 'api/Water'
                : selectedDataType === 'NaturalGas'
                    ? 'api/NaturalGas'
                    : "api/Paper";

    // Handle Data Type Change
    const handleDataTypeChange = (event) => {
        setSelectedDataType(event.target.value);
        setBuildingId('');
        setRecords([]);
    };

    // Fetch Buildings (only for Electric and Natural Gas)
    const fetchBuildings = async () => {
        try {
            const response = await axios.get(`${baseUrl}api/Building`);
            const filteredBuildings = response.data.buildings
                .filter((building) =>
                    selectedDataType === 'Electric'
                        ? building.e_MeterCode !== null
                        : building.g_MeterCode !== null
                )
                .map((building) => ({
                    id: building.id,
                    label: building.name,
                    value:
                        selectedDataType === 'Electric'
                            ? building.e_MeterCode
                            : building.g_MeterCode,
                }));
            setBuildings(filteredBuildings);
        } catch (error) {
            console.error('Error fetching buildings:', error);
        }
    };

    useEffect(() => {
        if (selectedDataType !== 'Water' && selectedDataType !== 'Paper')
            fetchBuildings();
    }, [selectedDataType]);

    // Validate Inputs
    const validateInputs = () => {
        const errors = {};
        const etcStartDate = new Date(startDate).toISOString();
        const etcEndDate = new Date(endDate).toISOString();

        if (!selectedDataType) {
            errors.dataType = 'Please select a consumption type.';
        }
        if (selectedDataType !== 'Water' &&  selectedDataType !== 'Paper' &&  !buildingId) {
            errors.building = 'Please select a building.';
        }
        if (!startDate) {
            errors.startDate = 'Start date is required.';
        }
        if (!endDate) {
            errors.endDate = 'End date is required.';
        }
        if (startDate && endDate && etcStartDate > etcEndDate) {
            errors.dateRange = 'Start date must be earlier than end date.';
        }

        setValidationErrors(errors);
        return Object.keys(errors).length === 0;
    };

    // Fetch Records
    const fetchRecords = async () => {
        if (!validateInputs()) {
            setValidated(false);
            return;
        }

        const etcStartDate = new Date(startDate).toISOString();
        const etcEndDate = new Date(endDate).toISOString();

        try {
            const params =
                selectedDataType === 'Water' || selectedDataType === 'Paper'
                    ? { startDate: etcStartDate, endDate: etcEndDate }
                    : { buildingId, startDate: etcStartDate, endDate: etcEndDate };

            const response = await axios.get(`${baseUrl}${endpoint}`, { params });
            switch (selectedDataType) {
                case 'Electric':
                    setRecords(response.data.electrics);
                    break;
                case 'Water':
                    setRecords(response.data.waters);
                    break;
                case 'NaturalGas':
                    setRecords(response.data.naturalGas);
                    break;
                case 'Paper':
                    setRecords(response.data.papers);
                default:
                    console.log('Invalid data type');
                    break;
            }
        } catch (error) {
            console.error('Error fetching records:', error);
        }
    };

    // Delete Record
    const deleteRecord = async () => {
        try {
            await axios.delete(`${baseUrl}${endpoint}/${recordToDelete}`,
                {
                    data: {
                        id: recordToDelete
                    }
                }
            );
            setRecords(records.filter((record) => record.id !== recordToDelete));
            setModalVisible(false);
            setRecordToDelete(null);
            
            axios.post(`${baseUrl}api/Prediction/train`, null, {
                params: {
                  consumptionType: selectedDataType.toLowerCase(),
                },
              }).then(() => {
                console.log('Model training started for consumption type');
              }).catch((err) => {
                console.error('Error starting training for consumption type:', err);
              });
        
              // If buildingId is available, send training request for the specific building
              if (recordToDelete.buildingId) {
                axios.post(`${baseUrl}api/Prediction/train`, {
                  consumptionType: selectedDataType.toLowerCase(),
                  buildingId: recordToDelete.buildingId,
                }).then(() => {
                  console.log('Model training started for specific building');
                }).catch((err) => {
                  console.error('Error starting training for specific building:', err);
                });
              }
        } catch (error) {
            console.error('Error deleting record:', error);
        }
    };

        // Handle Edit Button Click
        const handleEditClick = (record) => {
            setRecordToEdit({ ...record });
            setEditModalVisible(true);
        };
    
        // Validate Edit Form
        const validateForm = () => {
            const errors = {};
            let isValid = true;
    
            if (recordToEdit.initialMeterValue < 0) {
                errors.initialMeterValue = 'Initial Meter Value must be >= 0';
                isValid = false;
            }
            if (recordToEdit.finalMeterValue < 0) {
                errors.finalMeterValue = 'Final Meter Value must be >= 0';
                isValid = false;
            }
            if (Number(recordToEdit.finalMeterValue) < Number(recordToEdit.initialMeterValue)) {
                errors.finalMeterValue = 'Final Meter Value must be >= Initial Meter Value';
                isValid = false;
            }
            if (!recordToEdit.date) {
                errors.date = 'Date is required';
                isValid = false;
            }
    
            setValidationErrors(errors);
            return isValid;
        };
    
        // Submit Edit Form
        const handleEditSubmit = async () => {
            if (!validateForm()) return;
    
            try {

                recordToEdit.usage = recordToEdit.finalMeterValue - recordToEdit.initialMeterValue;
                if (selectedDataType === 'Electric') {
                    recordToEdit.kwhValue = recordToEdit.usage;
                } else if (selectedDataType === 'NaturalGas') {
                    recordToEdit.sM3Value = recordToEdit.usage;
                }
                const utcDate = new Date(recordToEdit.date).toISOString();
                recordToEdit.date = utcDate;

                await axios.put(`${baseUrl}${endpoint}/${recordToEdit.id}`, recordToEdit);
                setRecords(records.map((record) =>
                    record.id === recordToEdit.id ? recordToEdit : record
                ));
                setEditModalVisible(false);

                axios.post(`${baseUrl}api/Prediction/train`, null, {
                    params: {
                      consumptionType: selectedDataType.toLowerCase(),
                    },
                  }).then(() => {
                    console.log('Model training started for consumption type');
                  }).catch((err) => {
                    console.error('Error starting training for consumption type:', err);
                  });
            
                  // If buildingId is available, send training request for the specific building
                  if (recordToEdit.buildingId) {
                    axios.post(`${baseUrl}api/Prediction/train`, {
                      consumptionType: selectedDataType.toLowerCase(),
                      buildingId: recordToEdit.buildingId,
                    }).then(() => {
                      console.log('Model training started for specific building');
                    }).catch((err) => {
                      console.error('Error starting training for specific building:', err);
                    });
                  }
            } catch (error) {
                console.error('Error updating record:', error);
            }
        };

    return (
        <div>
            <h2>Record Management</h2>

            {/* Filter Form */}
            <CCard className="mb-4">
                <CCardHeader>Filter Records</CCardHeader>
                <CCardBody>
                    <CForm
                        className="row g-3"
                        noValidate
                        validated={validated}
                        onSubmit={(e) => {
                            e.preventDefault();
                            const isValid = validateInputs();
                            setValidated(true);
                            if (isValid) {
                                fetchRecords();
                            }
                        }}
                    >
                        <CCol md={2}>
                            <CFormSelect
                                id="dataType"
                                value={selectedDataType}
                                onChange={handleDataTypeChange}
                                label="Consumption Type"
                                invalid={!!validationErrors.dataType}
                                required
                            >
                                <option value="">Select Consumption Type</option>
                                <option value="Electric">Electric</option>
                                <option value="Water">Water</option>
                                <option value="NaturalGas">Natural Gas</option>
                                <option value="Paper">Paper</option>
                            </CFormSelect>
                            <CFormFeedback invalid>{validationErrors.dataType}</CFormFeedback>
                        </CCol>

                        {selectedDataType !== 'Water' && selectedDataType !== 'Paper' && (
                            <CCol md={4}>
                                <CFormSelect
                                    id="building"
                                    value={buildingId}
                                    onChange={(e) => setBuildingId(e.target.value)}
                                    label="Building"
                                    invalid={!!validationErrors.building}
                                    required
                                >
                                    <option value="">Select Building</option>
                                    {buildings.map((building) => (
                                        <option key={building.id} value={building.id}>
                                            {building.label}
                                        </option>
                                    ))}
                                </CFormSelect>
                                <CFormFeedback invalid>{validationErrors.building}</CFormFeedback>
                            </CCol>
                        )}

                        <CCol md={3}>
                            <CFormInput
                                type="date"
                                id="startDate"
                                label="Start Date"
                                value={startDate}
                                onChange={(e) => setStartDate(e.target.value)}
                                invalid={!!validationErrors.startDate}
                                required
                            />
                            <CFormFeedback invalid>{validationErrors.startDate}</CFormFeedback>
                        </CCol>

                        <CCol md={3}>
                            <CFormInput
                                type="date"
                                id="endDate"
                                label="End Date"
                                value={endDate}
                                onChange={(e) => setEndDate(e.target.value)}
                                invalid={!!validationErrors.endDate || !!validationErrors.dateRange}
                                required
                            />
                            <CFormFeedback invalid>{validationErrors.endDate}</CFormFeedback>
                            <CFormFeedback invalid>{validationErrors.dateRange}</CFormFeedback>
                        </CCol>

                        <CCol xs={12}>
                            <CButton color="primary" type="submit">
                                Fetch Records
                            </CButton>
                        </CCol>
                    </CForm>
                </CCardBody>
            </CCard>

            {/* Records Table */}
            {/* Records Table */}
            <CCard>
                <CCardHeader>Records</CCardHeader>
                <CCardBody>
                    <CTable>
                        <thead>
                            <tr>
                                <CTableHeaderCell>Date</CTableHeaderCell>
                                <CTableHeaderCell>Initial Meter Value</CTableHeaderCell>
                                <CTableHeaderCell>Final Meter Value</CTableHeaderCell>
                                <CTableHeaderCell>Usage</CTableHeaderCell>
                                <CTableHeaderCell>Actions</CTableHeaderCell>
                            </tr>
                        </thead>
                        <CTableBody>
                            {records.map((record) => (
                                <CTableRow key={record.id}>
                                    <CTableDataCell>{record.date}</CTableDataCell>
                                    <CTableDataCell>{record.initialMeterValue}</CTableDataCell>
                                    <CTableDataCell>{record.finalMeterValue}</CTableDataCell>
                                    <CTableDataCell>{record.usage}</CTableDataCell>
                                    <CTableDataCell>
                                        <CButton
                                            color="warning"
                                            variant="outline"
                                            onClick={() => handleEditClick(record)}
                                            style={{ marginRight: '8px' }}
                                        >
                                            <CIcon icon={cilPencil} />
                                        </CButton>
                                        <CButton
                                            color="danger"
                                            variant="outline"
                                            onClick={() => {
                                                setRecordToDelete(record.id);
                                                setModalVisible(true);
                                            }}
                                        >
                                            <CIcon icon={cilTrash} />
                                        </CButton>
                                    </CTableDataCell>
                                </CTableRow>
                            ))}
                        </CTableBody>
                    </CTable>
                </CCardBody>
            </CCard>

            {/* Delete Confirmation Modal */}
            <CModal visible={modalVisible} onClose={() => setModalVisible(false)}>
                <CModalHeader>Confirm Deletion</CModalHeader>
                <CModalBody>Are you sure you want to delete this record?</CModalBody>
                <CModalFooter>
                    <CButton color="secondary" onClick={() => setModalVisible(false)}>Cancel</CButton>
                    <CButton color="danger" onClick={deleteRecord}>Delete</CButton>
                </CModalFooter>
            </CModal>

            {/* Edit Modal */}
            <CModal visible={editModalVisible} onClose={() => setEditModalVisible(false)}>
                <CModalHeader>Edit Record</CModalHeader>
                <CModalBody>
                    <CForm>
                        <CFormInput
                            type="date"
                            label="Date"
                            value={recordToEdit?.date || ''}
                            onChange={(e) => setRecordToEdit({ ...recordToEdit, date: e.target.value })}
                            invalid={!!validationErrors.date}
                        />
                        <CFormFeedback invalid>{validationErrors.date}</CFormFeedback>

                        <CFormInput
                            type="number"
                            label="Initial Meter Value"
                            value={recordToEdit?.initialMeterValue || ''}
                            onChange={(e) => setRecordToEdit({ ...recordToEdit, initialMeterValue: e.target.value })}
                            invalid={!!validationErrors.initialMeterValue}
                        />
                        <CFormFeedback invalid>{validationErrors.initialMeterValue}</CFormFeedback>

                        <CFormInput
                            type="number"
                            label="Final Meter Value"
                            value={recordToEdit?.finalMeterValue || ''}
                            onChange={(e) => setRecordToEdit({ ...recordToEdit, finalMeterValue: e.target.value })}
                            invalid={!!validationErrors.finalMeterValue}
                        />
                        <CFormFeedback invalid>{validationErrors.finalMeterValue}</CFormFeedback>
                    </CForm>
                </CModalBody>
                <CModalFooter>
                    <CButton color="secondary" onClick={() => setEditModalVisible(false)}>Cancel</CButton>
                    <CButton color="success" onClick={handleEditSubmit}>Save Changes</CButton>
                </CModalFooter>
            </CModal>
        </div>
    );
};

export default DeleteRecordPage;
