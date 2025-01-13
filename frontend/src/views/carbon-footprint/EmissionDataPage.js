import React, { useState, useEffect } from 'react';
import { CCard, CCardBody, CContainer, CRow, CCol, CCardHeader, CProgress, CAlert, CButton } from '@coreui/react';
import { CChartBar } from '@coreui/react-chartjs';

const EmissionDataPage = () => {
  const [emissionData, setEmissionData] = useState({
    year: 2024,
    electricityEmission: 0,
    shuttleBusEmission: 0,
    carEmission: 0,
    motorcycleEmission: 0,
    totalEmission: 0,
  });

  const [chartData, setChartData] = useState({
    labels: [],
    datasets: [
      {
        label: 'Electricity Emission',
        data: [],
        backgroundColor: 'rgba(75, 192, 192, 0.5)',
      },
      {
        label: 'Shuttle Bus Emission',
        data: [],
        backgroundColor: 'rgba(153, 102, 255, 0.5)',
      },
      {
        label: 'Car Emission',
        data: [],
        backgroundColor: 'rgba(255, 159, 64, 0.5)',
      },
      {
        label: 'Motorcycle Emission',
        data: [],
        backgroundColor: 'rgba(255, 99, 132, 0.5)',
      },
    ],
  });

  // Fetch the emission data from the API
  useEffect(() => {
    const fetchEmissionData = async () => {
      const response = await fetch('http://localhost:5154/api/CarbonFootprint/all');
      const data = await response.json();

      // Set the latest data to state
      const latestElement = data.yearlyFootprints.length - 1; // Get the index of the latest year
      const latestData = data.yearlyFootprints[latestElement]; // Get the latest year data
      setEmissionData(latestData);

      // Prepare chart data
      const years = data.yearlyFootprints.map((item) => item.year);
      const electricityData = data.yearlyFootprints.map((item) => item.electricityEmission);
      const shuttleBusData = data.yearlyFootprints.map((item) => item.shuttleBusEmission);
      const carData = data.yearlyFootprints.map((item) => item.carEmission);
      const motorcycleData = data.yearlyFootprints.map((item) => item.motorcycleEmission);

      setChartData({
        labels: years,
        datasets: [
          { ...chartData.datasets[0], data: electricityData },
          { ...chartData.datasets[1], data: shuttleBusData },
          { ...chartData.datasets[2], data: carData },
          { ...chartData.datasets[3], data: motorcycleData },
        ],
      });
    };

    fetchEmissionData();
  }, []);

  return (
    <CContainer className="py-4">
      <CRow className="justify-content-center">
        <CCol md={8}>
          <CCard>
            <CCardHeader>
              <h4>Emission Data for Year {emissionData.year}</h4>
            </CCardHeader>
            <CCardBody>
              {/* Total Emission as a Progress Bar */}
              <CRow className="mb-4">
                <CCol>
                  <h6>Total Emission (kg CO₂)</h6>
                  <CProgress value={emissionData.totalEmission / 2000 * 100} color="success" />
                  <p className="mt-2">{emissionData.totalEmission.toFixed(2)} kg CO₂</p>
                </CCol>
              </CRow>

              {/* Electricity Emission */}
              <CRow className="mb-3">
                <CCol>
                  <CAlert color="info">
                    <h6>Electricity Emission</h6>
                    <p>{emissionData.electricityEmission.toFixed(2)} kg CO₂</p>
                  </CAlert>
                </CCol>
              </CRow>

              {/* Shuttle Bus Emission */}
              <CRow className="mb-3">
                <CCol>
                  <CAlert color="primary">
                    <h6>Shuttle Bus Emission</h6>
                    <p>{emissionData.shuttleBusEmission.toFixed(2)} kg CO₂</p>
                  </CAlert>
                </CCol>
              </CRow>

              {/* Car Emission */}
              <CRow className="mb-3">
                <CCol>
                  <CAlert color="warning">
                    <h6>Car Emission</h6>
                    <p>{emissionData.carEmission.toFixed(2)} kg CO₂</p>
                  </CAlert>
                </CCol>
              </CRow>

              {/* Motorcycle Emission */}
              <CRow className="mb-3">
                <CCol>
                  <CAlert color="danger">
                    <h6>Motorcycle Emission</h6>
                    <p>{emissionData.motorcycleEmission.toFixed(2)} kg CO₂</p>
                  </CAlert>
                </CCol>
              </CRow>

              {/* Bar Chart using CChartBar */}
              <CRow className="mt-4">
                <CCol>
                  <CChartBar
                    data={chartData}
                    options={{
                      responsive: true,
                      plugins: {
                        title: {
                          display: true,
                          text: 'Yearly Carbon Footprints',
                        },
                        legend: {
                          position: 'top',
                        },
                      },
                      scales: {
                        x: {
                          title: {
                            display: true,
                            text: 'Year',
                          },
                        },
                        y: {
                          title: {
                            display: true,
                            text: 'Emission (kg CO₂)',
                          },
                        },
                      },
                    }}
                  />
                </CCol>
              </CRow>

              {/* Refresh Button */}
              <CRow className="justify-content-center">
                <CCol xs={6} className="d-flex justify-content-center">
                  <CButton
                    color="primary"
                    onClick={() => window.location.reload()}
                    className="px-4"
                  >
                    Refresh Data
                  </CButton>
                </CCol>
              </CRow>
            </CCardBody>
          </CCard>
        </CCol>
      </CRow>
    </CContainer>
  );
};

export default EmissionDataPage;
