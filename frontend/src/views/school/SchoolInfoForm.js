import React, { useState } from 'react';
import {
  CButton,
  CCard,
  CCardBody,
  CContainer,
  CForm,
  CFormInput,
  CRow,
  CCol,
  CFormLabel,
  CFormFeedback,
  CFormText,
} from '@coreui/react';
import axios from 'axios';

const SchoolInfoForm = () => {
  const [formData, setFormData] = useState({
    numberOfPeople: '',
    year: '',
    carsManagedByUniversity: '',
    carsEnteringUniversity: '',
    motorcyclesEnteringUniversity: '',
  });

  const [formErrors, setFormErrors] = useState({
    numberOfPeople: '',
    year: '',
    carsManagedByUniversity: '',
    carsEnteringUniversity: '',
    motorcyclesEnteringUniversity: '',
  });

  const [isSubmitted, setIsSubmitted] = useState(false);
  const baseUrl = 'http://localhost:5154';

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const validateForm = () => {
    let errors = {};
    let isValid = true;

    // Validate all fields
    if (!formData.numberOfPeople || isNaN(formData.numberOfPeople)) {
      isValid = false;
      errors.numberOfPeople = 'Please enter a valid number of people';
    }
    if (!formData.year || isNaN(formData.year)) {
      isValid = false;
      errors.year = 'Please enter a valid year';
    }
    if (!formData.carsManagedByUniversity || isNaN(formData.carsManagedByUniversity)) {
      isValid = false;
      errors.carsManagedByUniversity = 'Please enter a valid number of cars managed by the university';
    }
    if (!formData.carsEnteringUniversity || isNaN(formData.carsEnteringUniversity)) {
      isValid = false;
      errors.carsEnteringUniversity = 'Please enter a valid number of cars entering the university';
    }
    if (!formData.motorcyclesEnteringUniversity || isNaN(formData.motorcyclesEnteringUniversity)) {
      isValid = false;
      errors.motorcyclesEnteringUniversity = 'Please enter a valid number of motorcycles entering the university';
    }

    setFormErrors(errors);
    return isValid;
  };


  const handleSubmit = async (e) => {
    e.preventDefault();
    if (validateForm()) {
      setIsSubmitted(true);
      try {
        // Send form data to the backend using Axios
        const response = await axios.post(baseUrl + '/api/SchoolInfo', formData);
  
        // Handle success response from the server
        console.log(response.data.message); // Log success message
  
      } catch (error) {
        // Handle error response from the server
        if (error.response) {
          console.error('Error:', error.response.data.message); // Log error message
        } else {
          console.error('Error submitting form:', error.message); // Log other errors (e.g., network issues)
        }
      }
    } else {
      setIsSubmitted(false);
    }
  };
  

  return (
    <CContainer className="py-4">
      <CRow className="justify-content-center">
        <CCol md={8}>
          <CCard>
            <CCardBody>
              <CForm onSubmit={handleSubmit}>
                <h1>Vehicle Management Form</h1>

                {/* Number of People */}
                <CRow className="mb-4">
                  <CCol>
                    <CFormLabel htmlFor="numberOfPeople">Number of People</CFormLabel>
                    <CFormInput
                      type="number"
                      id="numberOfPeople"
                      name="numberOfPeople"
                      value={formData.numberOfPeople}
                      onChange={handleChange}
                      invalid={!!formErrors.numberOfPeople}
                    />
                    {formErrors.numberOfPeople && <CFormFeedback invalid>{formErrors.numberOfPeople}</CFormFeedback>}
                  </CCol>
                </CRow>

                {/* Year */}
                <CRow className="mb-4">
                  <CCol>
                    <CFormLabel htmlFor="year">Year</CFormLabel>
                    <CFormInput
                      type="number"
                      id="year"
                      name="year"
                      value={formData.year}
                      onChange={handleChange}
                      invalid={!!formErrors.year}
                    />
                    {formErrors.year && <CFormFeedback invalid>{formErrors.year}</CFormFeedback>}
                  </CCol>
                </CRow>

                {/* Cars Managed by University */}
                <CRow className="mb-4">
                  <CCol>
                    <CFormLabel htmlFor="carsManagedByUniversity">Cars Managed by University</CFormLabel>
                    <CFormInput
                      type="number"
                      id="carsManagedByUniversity"
                      name="carsManagedByUniversity"
                      value={formData.carsManagedByUniversity}
                      onChange={handleChange}
                      invalid={!!formErrors.carsManagedByUniversity}
                    />
                    {formErrors.carsManagedByUniversity && <CFormFeedback invalid>{formErrors.carsManagedByUniversity}</CFormFeedback>}
                  </CCol>
                </CRow>

                {/* Cars Entering University */}
                <CRow className="mb-4">
                  <CCol>
                    <CFormLabel htmlFor="carsEnteringUniversity">Cars Entering University</CFormLabel>
                    <CFormInput
                      type="number"
                      id="carsEnteringUniversity"
                      name="carsEnteringUniversity"
                      value={formData.carsEnteringUniversity}
                      onChange={handleChange}
                      invalid={!!formErrors.carsEnteringUniversity}
                    />
                    {formErrors.carsEnteringUniversity && <CFormFeedback invalid>{formErrors.carsEnteringUniversity}</CFormFeedback>}
                  </CCol>
                </CRow>

                {/* Motorcycles Entering University */}
                <CRow className="mb-4">
                  <CCol>
                    <CFormLabel htmlFor="motorcyclesEnteringUniversity">Motorcycles Entering University</CFormLabel>
                    <CFormInput
                      type="number"
                      id="motorcyclesEnteringUniversity"
                      name="motorcyclesEnteringUniversity"
                      value={formData.motorcyclesEnteringUniversity}
                      onChange={handleChange}
                      invalid={!!formErrors.motorcyclesEnteringUniversity}
                    />
                    {formErrors.motorcyclesEnteringUniversity && <CFormFeedback invalid>{formErrors.motorcyclesEnteringUniversity}</CFormFeedback>}
                  </CCol>
                </CRow>

                {/* Submit Button */}
                <CRow className="justify-content-center">
                  <CCol xs={6} className="d-flex justify-content-center">
                    <CButton type="submit" color="primary" className="px-4">
                      Submit
                    </CButton>
                  </CCol>
                </CRow>

                {isSubmitted && (
                  <CRow className="mt-3">
                    <CCol>
                      <CFormText className="text-success">Form submitted successfully!</CFormText>
                    </CCol>
                  </CRow>
                )}
              </CForm>
            </CCardBody>
          </CCard>
        </CCol>
      </CRow>
    </CContainer>
  );
};

export default SchoolInfoForm;
