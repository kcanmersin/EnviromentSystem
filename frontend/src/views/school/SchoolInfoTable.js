import React, { useEffect, useState } from 'react';
import { CTable, CTableBody, CTableDataCell, CTableHead, CTableHeaderCell, CTableRow, CButton, CCard , CCardHeader, CCardBody} from '@coreui/react';
import CIcon from '@coreui/icons-react';
import { cilPencil, cilTrash } from '@coreui/icons';
import { fetchSchoolInfo, deleteSchoolInfo, updateSchoolInfo } from '../../services/school-services/api.js';
import UpdateModal from './UpdateModal';

const SchoolInfoTable = () => {
    const [data, setData] = useState([]);
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedData, setSelectedData] = useState(null);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        const result = await fetchSchoolInfo();
        setData(result);
    };

    const handleDelete = async (id) => {
        if (window.confirm('Are you sure you want to delete this record?')) {
            await deleteSchoolInfo(id);
            loadData();
        }
    };

    const handleUpdate = (item) => {
        setSelectedData(item);
        setModalVisible(true);
    };

    const handleSave = async (updatedItem) => {
        await updateSchoolInfo(updatedItem);
        setModalVisible(false);
        loadData();
    };

    return (
        <CCard>
            <CCardHeader>
                School Info Table
            </CCardHeader>
            <CCardBody>
                <CTable striped hover>
                    <CTableHead>
                        <CTableRow>
                            <CTableHeaderCell>Year</CTableHeaderCell>
                            <CTableHeaderCell>People</CTableHeaderCell>
                            <CTableHeaderCell>Cars Managed</CTableHeaderCell>
                            <CTableHeaderCell>Cars Entering</CTableHeaderCell>
                            <CTableHeaderCell>Motorcycles Entering</CTableHeaderCell>
                            <CTableHeaderCell>Actions</CTableHeaderCell>
                        </CTableRow>
                    </CTableHead>
                    <CTableBody>
                        {data.map((item) => (
                            <CTableRow key={item.id}>
                                <CTableDataCell>{item.year}</CTableDataCell>
                                <CTableDataCell>{item.numberOfPeople}</CTableDataCell>
                                <CTableDataCell>{item.carsManagedByUniversity}</CTableDataCell>
                                <CTableDataCell>{item.carsEnteringUniversity}</CTableDataCell>
                                <CTableDataCell>{item.motorcyclesEnteringUniversity}</CTableDataCell>
                                <CTableDataCell>
                                    <CButton color="warning" onClick={() => handleUpdate(item)}>
                                        <CIcon icon={cilPencil} />
                                    </CButton>
                                    <CButton color="danger" onClick={() => handleDelete(item.id)}>
                                        <CIcon icon={cilTrash} />
                                    </CButton>
                                </CTableDataCell>
                            </CTableRow>
                        ))}
                    </CTableBody>
                </CTable>

                <UpdateModal
                    visible={modalVisible}
                    onClose={() => setModalVisible(false)}
                    onSave={handleSave}
                    selectedData={selectedData}
                />
            </CCardBody>
        </CCard>

    );
};

export default SchoolInfoTable;
