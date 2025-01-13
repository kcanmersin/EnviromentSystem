import React, { useState, useEffect } from 'react';
import axios from 'axios';
import {
  CContainer, CRow, CCol, CCard, CCardBody, CForm, CFormLabel, CFormSelect,
  CFormInput, CInputGroup, CInputGroupText, CButton, CFormFeedback
} from '@coreui/react';
import CIcon from '@coreui/icons-react';
import { cilCalendar, cilSpeedometer, cilBolt, cilDrop } from '@coreui/icons';

const ConsumptionInputPage = () => {
  const [selectedDataType, setSelectedDataType] = useState('Electric');
  const [date, setDate] = useState('');
  const [initialMeterValue, setInitialMeterValue] = useState('');
  const [finalMeterValue, setFinalMeterValue] = useState('');
  const [usage, setUsage] = useState('');
  const [value, setValue] = useState('');
  const [selectedBuilding, setSelectedBuilding] = useState('');
  const [electricBuildings, setElectricBuildings] = useState([]);
  const [naturalGasBuildings, setNaturalGasBuildings] = useState([]);
  const [validated, setValidated] = useState(false);
  const [validationErrors, setValidationErrors] = useState({});

  const baseUrl = 'http://localhost:5154/';

  useEffect(() => {
    fetchBuildings();
  }, []);

  useEffect(() => {
    if (initialMeterValue && finalMeterValue) {
      const calculatedUsage = finalMeterValue - initialMeterValue;
      setUsage(calculatedUsage >= 0 ? calculatedUsage : 0);
      setValue(calculatedUsage >= 0 ? calculatedUsage : 0);
    }
  }, [initialMeterValue, finalMeterValue]);

  const fetchBuildings = async () => {
    try {
      const response = await axios.get(baseUrl + 'api/Building');
      const electricBuildings = response.data.buildings
        .filter(building => building.e_MeterCode !== null)
        .map(building => ({ id: building.id, label: building.name, value: building.e_MeterCode }));
      const naturalGasBuildings = response.data.buildings
        .filter(building => building.g_MeterCode !== null)
        .map(building => ({ id: building.id, label: building.name, value: building.g_MeterCode }));

      setElectricBuildings(electricBuildings);
      setNaturalGasBuildings(naturalGasBuildings);
    } catch (error) {
      console.error('Error fetching buildings:', error);
    }
  };

  const validateForm = () => {
    const errors = {};
    let isValid = true;

    if (!initialMeterValue || initialMeterValue < 0) {
      errors.initialMeterValue = 'Initial Meter Value must be greater than or equal to 0';
      isValid = false;
    }
    if (!finalMeterValue || finalMeterValue < 0) {
      errors.finalMeterValue = 'Final Meter Value must be greater than or equal to 0';
      isValid = false;
    }
    if (Number(finalMeterValue) < Number(initialMeterValue)) {
      errors.finalMeterValue = 'Final Meter Value must be greater than or equal to Initial Meter Value';
      isValid = false;
    }
    if (!date) {
      errors.date = 'Date is required';
      isValid = false;
    }
    if (selectedDataType !== 'Water' && !selectedBuilding) {
      errors.selectedBuilding = 'Please select a building';
      isValid = false;
    }

    setValidationErrors(errors);
    return isValid;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const isValid = validateForm();

    if (!isValid) {
      setValidated(false);
      return;
    }

    const utcDate = new Date(date).toISOString();
    const consumptionValue = selectedDataType !== 'Water' ? value : '';

    const data = {
      date: utcDate,
      initialMeterValue,
      finalMeterValue,
      usage,
      [selectedDataType === 'Electric' ? 'kWhValue' : selectedDataType === 'NaturalGas' ? 'sM3Value' : '']: consumptionValue,
      buildingId: selectedBuilding,
    };

    try {
      const endpoint =
        selectedDataType === 'Electric'
          ? 'api/Electric'
          : selectedDataType === 'Water'
            ? 'api/Water'
            : 'api/NaturalGas';

      const response = await axios.post(baseUrl + endpoint, data);
      console.log('Form submitted successfully:', response.data);

      // Send training request for overall consumption type
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
      if (selectedBuilding) {
        axios.post(`${baseUrl}api/Prediction/train`, {
          consumptionType: selectedDataType.toLowerCase(),
          buildingId: selectedBuilding,
        }).then(() => {
          console.log('Model training started for specific building');
        }).catch((err) => {
          console.error('Error starting training for specific building:', err);
        });
      }
    } catch (error) {
      console.error('Error submitting form:', error);

    }
  };

  const handleDataTypeChange = (e) => {
    setSelectedDataType(e.target.value);
    setInitialMeterValue('');
    setFinalMeterValue('');
    setUsage('');
    setValue('');
    setSelectedBuilding('');
  };

  return (
    <CContainer className="py-4">
      <CRow className="justify-content-center">
        <CCol md={8}>
          <CCard>
            <CCardBody>
              <CForm
                className="row g-3 needs-validation"
                noValidate
                validated={validated}
                onSubmit={handleSubmit}
              >
                <h1>Enter Consumption Data</h1>

                {/* Data Type Selection */}
                <CFormLabel htmlFor="dataType">Select Data Type</CFormLabel>
                <CFormSelect id="dataType" value={selectedDataType} onChange={handleDataTypeChange}>
                  <option value="Electric">Electric</option>
                  <option value="Water">Water</option>
                  <option value="NaturalGas">Natural Gas</option>
                </CFormSelect>

                {/* Building Selection */}
                {selectedDataType !== 'Water' && (
                  <>
                    <CFormLabel htmlFor="building">Building</CFormLabel>
                    <CFormSelect
                      id="building"
                      value={selectedBuilding}
                      onChange={(e) => setSelectedBuilding(e.target.value)}
                      invalid={!!validationErrors.selectedBuilding}
                    >
                      <option value="" disabled>
                        Select a Building
                      </option>
                      {(selectedDataType === 'Electric'
                        ? electricBuildings
                        : naturalGasBuildings
                      ).map((building) => (
                        <option key={building.id} value={building.id}>
                          {building.label}
                        </option>
                      ))}
                    </CFormSelect>
                    {validationErrors.selectedBuilding && (
                      <CFormFeedback invalid>{validationErrors.selectedBuilding}</CFormFeedback>
                    )}
                  </>
                )}

                {/* Date */}
                <CFormLabel htmlFor="date">Date</CFormLabel>
                <CInputGroup>
                  <CInputGroupText>
                    <CIcon icon={cilCalendar} />
                  </CInputGroupText>
                  <CFormInput
                    id="date"
                    type="date"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                    invalid={!!validationErrors.date}
                  />
                  {validationErrors.date && <CFormFeedback invalid>{validationErrors.date}</CFormFeedback>}
                </CInputGroup>

                {/* Initial Meter Value */}
                <CFormLabel htmlFor="initialMeterValue">Initial Meter Value</CFormLabel>
                <CInputGroup>
                  <CInputGroupText>
                    <CIcon icon={cilSpeedometer} />
                  </CInputGroupText>
                  <CFormInput
                    id="initialMeterValue"
                    type="number"
                    value={initialMeterValue}
                    onChange={(e) => setInitialMeterValue(e.target.value)}
                    invalid={!!validationErrors.initialMeterValue}
                  />
                  {validationErrors.initialMeterValue && (
                    <CFormFeedback invalid>{validationErrors.initialMeterValue}</CFormFeedback>
                  )}
                </CInputGroup>

                {/* Final Meter Value */}
                <CFormLabel htmlFor="finalMeterValue">Final Meter Value</CFormLabel>
                <CInputGroup>
                  <CInputGroupText>
                    <CIcon icon={cilSpeedometer} />
                  </CInputGroupText>
                  <CFormInput
                    id="finalMeterValue"
                    type="number"
                    value={finalMeterValue}
                    onChange={(e) => setFinalMeterValue(e.target.value)}
                    invalid={!!validationErrors.finalMeterValue}
                  />
                  {validationErrors.finalMeterValue && (
                    <CFormFeedback invalid>{validationErrors.finalMeterValue}</CFormFeedback>
                  )}
                </CInputGroup>

                {/* Usage */}
                <CFormLabel htmlFor="usage">Usage</CFormLabel>
                <CInputGroup>
                  <CInputGroupText>
                    <CIcon icon={cilSpeedometer} />
                  </CInputGroupText>
                  <CFormInput id="usage" type="number" value={usage} disabled />
                </CInputGroup>

                {/* Value */}
                <CFormLabel htmlFor="value">Value</CFormLabel>
                <CInputGroup>
                  <CInputGroupText>
                    {selectedDataType === 'Electric' ? (
                      <CIcon icon={cilBolt} />
                    ) : selectedDataType === 'NaturalGas' ? (
                      <CIcon icon={cilDrop} />
                    ) : null}
                  </CInputGroupText>
                  <CFormInput
                    id="value"
                    type="number"
                    value={value}
                    disabled
                  />
                </CInputGroup>




                {/* Submit */}
                <CRow className="justify-content-center">
                  <CCol xs={6} className="d-flex justify-content-center">
                    <CButton type="submit" color="primary" className="px-4 mt-4">
                      Submit
                    </CButton>
                  </CCol>
                </CRow>
              </CForm>
            </CCardBody>
          </CCard>
        </CCol>
      </CRow>
    </CContainer>
  );
};

export default ConsumptionInputPage;
