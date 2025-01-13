import React, { useState, useEffect } from "react";
import {
  CRow,
  CCol,
  CCard,
  CCardBody,
  CButton,
  CCardHeader,
  CModal,
  CModalHeader,
  CModalTitle,
  CModalBody,
  CModalFooter,
  CFormInput,
  CFormLabel,
  CForm,
  CAlert,  // Import CAlert for notifications
} from "@coreui/react";
import { CChartLine } from "@coreui/react-chartjs";
import axios from "axios";
import ReportSection from '../report/ReportSection';

const PaperConsumption = () => {
  const [chartData, setChartData] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [newUsage, setNewUsage] = useState('');
  const [newDate, setNewDate] = useState('');
  const [notification, setNotification] = useState(null);  // State to manage notifications

  // Fetch Paper Data
  const fetchPaperData = async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await axios.get("http://localhost:5154/api/Paper");
      const papers = response.data.papers;

      // Sort data by date (ascending)
      const sortedData = papers.sort((a, b) => new Date(a.date) - new Date(b.date));

      // Prepare data for the chart
      const dates = sortedData.map(entry =>
        new Date(entry.date).toLocaleDateString('en-GB', { year: 'numeric', month: 'short', day: 'numeric' })
      );
      const usageData = sortedData.map(entry => entry.usage);

      setChartData({
        labels: dates,
        datasets: [
          {
            label: "Paper Consumption Over Time",
            borderColor: "#28a745",
            backgroundColor: "rgba(40, 167, 69, 0.1)",
            data: usageData,
            fill: true,
            tension: 0.4, // Smooth line
          },
        ],
      });

      setLoading(false);
    } catch (error) {
      console.error("Error fetching paper data:", error);
      setError("Failed to load paper data.");
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPaperData();
  }, []);

  // Handle Add New Record
  const handleAddRecord = async (e) => {
    e.preventDefault();

    if (!newDate || !newUsage) {
      setNotification({ type: "danger", message: "Both date and usage are required." });
      return;
    }

    try {
      const data = {
        date: new Date(newDate).toISOString(),
        usage: parseFloat(newUsage),
      };

      await axios.post("http://localhost:5154/api/Paper", data);
      setModalVisible(false);
      setNewDate('');
      setNewUsage('');
      fetchPaperData();
      setNotification({ type: "success", message: "New paper consumption record added successfully!" });
    } catch (error) {
      console.error("Error adding new record:", error);
      setNotification({ type: "danger", message: "Failed to add new record." });
    }
  };

  return (
    <div>
      <ReportSection type="paper" />

      <CCard>
        <CCardHeader>
          <CRow className="mb-3">
            <CCol xs="auto">
              <h4>Paper Consumption Over Time</h4>
            </CCol>

            <CCol xs="auto">
              <CButton color="success" onClick={() => setModalVisible(true)}>
                Add Record
              </CButton>
            </CCol>
          </CRow>
        </CCardHeader>

        <CCardBody>
          {loading ? (
            <p>Loading paper data...</p>
          ) : error ? (
            <p className="text-danger">{error}</p>
          ) : (
            <CChartLine
              data={chartData}
              options={{
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                  legend: {
                    labels: {
                      color: "#000",
                    },
                  },
                  tooltip: {
                    callbacks: {
                      label: (context) => `${context.raw} kg`,
                    },
                  },
                },
                scales: {
                  x: {
                    title: {
                      display: true,
                      text: 'Date',
                      color: "#000",
                    },
                    ticks: {
                      color: "#000",
                    },
                  },
                  y: {
                    title: {
                      display: true,
                      text: 'Usage (kg)',
                      color: "#000",
                    },
                    ticks: {
                      color: "#000",
                    },
                  },
                },
              }}
              style={{ height: "400px" }}
            />
          )}

          {/* Display Notification using CAlert */}
          {notification && (
            <CAlert color={notification.type} className="mt-3">
              {notification.message}
            </CAlert>
          )}
        </CCardBody>
      </CCard>

      {/* Modal for Adding New Record */}
      <CModal visible={modalVisible} onClose={() => setModalVisible(false)}>
        <CModalHeader closeButton>
          <CModalTitle>Add New Paper Consumption Record</CModalTitle>
        </CModalHeader>
        <CModalBody>
          <CForm onSubmit={handleAddRecord}>
            <CFormLabel htmlFor="newDate">Date</CFormLabel>
            <CFormInput
              type="date"
              id="newDate"
              value={newDate}
              onChange={(e) => setNewDate(e.target.value)}
              required
            />

            <CFormLabel htmlFor="newUsage" className="mt-3">
              Usage (kg)
            </CFormLabel>
            <CFormInput
              type="number"
              id="newUsage"
              value={newUsage}
              onChange={(e) => setNewUsage(e.target.value)}
              required
            />

            <CModalFooter className="mt-3">
              <CButton type="submit" color="primary">
                Save
              </CButton>
              <CButton color="secondary" onClick={() => setModalVisible(false)}>
                Cancel
              </CButton>
            </CModalFooter>
          </CForm>
        </CModalBody>
      </CModal>
    </div>
  );
};

export default PaperConsumption;
