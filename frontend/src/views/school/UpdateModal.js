import React, { useState, useEffect } from 'react';
import { CModal, CModalHeader, CModalBody, CModalFooter, CButton, CFormInput, CFormLabel, CForm } from '@coreui/react';

const UpdateModal = ({ visible, onClose, onSave, selectedData }) => {
  const [formData, setFormData] = useState({
    id: '',
    numberOfPeople: '',
    year: '',
    carsManagedByUniversity: '',
    carsEnteringUniversity: '',
    motorcyclesEnteringUniversity: '',
  });

  useEffect(() => {
    if (selectedData) {
      setFormData(selectedData);
    }
  }, [selectedData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = () => {
    const isValid = Object.values(formData).every((field) => field !== '');
    if (!isValid) {
      alert('All fields must be filled!');
      return;
    }
    onSave(formData);
  };

  return (
    <CModal visible={visible} onClose={onClose}>
      <CModalHeader>Update School Info</CModalHeader>
      <CModalBody>
        <CForm>
          <CFormLabel>Number of People</CFormLabel>
          <CFormInput type="number" name="numberOfPeople" value={formData.numberOfPeople} onChange={handleChange} />
          
          <CFormLabel>Year</CFormLabel>
          <CFormInput type="number" name="year" value={formData.year} onChange={handleChange} />

          <CFormLabel>Cars Managed By University</CFormLabel>
          <CFormInput type="number" name="carsManagedByUniversity" value={formData.carsManagedByUniversity} onChange={handleChange} />

          <CFormLabel>Cars Entering University</CFormLabel>
          <CFormInput type="number" name="carsEnteringUniversity" value={formData.carsEnteringUniversity} onChange={handleChange} />

          <CFormLabel>Motorcycles Entering University</CFormLabel>
          <CFormInput type="number" name="motorcyclesEnteringUniversity" value={formData.motorcyclesEnteringUniversity} onChange={handleChange} />
        </CForm>
      </CModalBody>
      <CModalFooter>
        <CButton color="secondary" onClick={onClose}>Cancel</CButton>
        <CButton color="primary" onClick={handleSubmit}>Save</CButton>
      </CModalFooter>
    </CModal>
  );
};

export default UpdateModal;
