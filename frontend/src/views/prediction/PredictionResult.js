import React, { useState, useEffect } from "react";
import {
  CRow,
  CCol,
  CCard,
  CCardHeader,
  CCardBody,
  CForm,
  CFormLabel,
  CFormInput,
  CInputGroup,
  CFormSelect,
  CButton,
  CFormFeedback,
} from "@coreui/react";
import { CChartBar, CChartLine } from "@coreui/react-chartjs";
import axios from "axios";

const PredictionResult = () => {
  const [consumptionType, setConsumptionType] = useState("electric");
  const [buildings, setBuildings] = useState([]);
  const [electricBuildings, setElectricBuildings] = useState([]);
  const [gasBuildings, setGasBuildings] = useState([]);
  const [buildingId, setBuildingId] = useState("");
  const [numOfMonths, setNumOfMonths] = useState(3);
  const [startDate, setStartDate] = useState("");
  const [realConsumptionData, setRealConsumptionData] = useState([]);
  const [predictedData, setPredictedData] = useState([]);
  const [formErrors, setFormErrors] = useState({});
  const [validated, setValidated] = useState(false);

  const fetchElectricBuildings = async () => {
    try {
      const response = await axios.get("http://localhost:5154/api/Building");
      const filteredBuildings = response.data.buildings
        .filter((building) => building.e_MeterCode !== null)
        .map((building) => ({
          id: building.id,
          label: building.name,
          value: building.e_MeterCode,
        }));
      setElectricBuildings(filteredBuildings);
    } catch (error) {
      console.error("Error fetching electric buildings:", error);
    }
  };

  const fetchNaturalGasBuildings = async () => {
    try {
      const response = await axios.get("http://localhost:5154/api/Building");
      const filteredBuildings = response.data.buildings
        .filter((building) => building.g_MeterCode !== null)
        .map((building) => ({
          id: building.id,
          label: building.name,
          value: building.g_MeterCode,
        }));
      setGasBuildings(filteredBuildings);
    } catch (error) {
      console.error("Error fetching gas buildings:", error);
    }
  };

  // Fetch total or individual real consumption data
  const fetchRealConsumptionData = async () => {
    try {
      const endpoint = `http://localhost:5154/api/${consumptionType === "electric" ? "Electric" : "NaturalGas"}`;
      const etcDate = new Date(startDate).toISOString();
      const params = buildingId
        ? { buildingId, startDate: etcDate }
        : { startDate: etcDate };

      const response = await axios.get(endpoint, { params });

      if (response.status === 200 && response.data) {
        const dataKey = consumptionType === "electric" ? "electrics" : "naturalGas";
        const consumptionData = response.data[dataKey];

        // Get the last date in the data (last entry's date)
        const lastDate = new Date(consumptionData[consumptionData.length - 1].date);
        const startDateObj = new Date(startDate);

        // Calculate the number of months between startDate and the lastDate in the data
        const monthDifference = (lastDate.getFullYear() - startDateObj.getFullYear()) * 12 + lastDate.getMonth() - startDateObj.getMonth();

        // Create an array for monthly consumption data (based on month difference)
        const monthlyConsumption = new Array(monthDifference + 1).fill(0);

        // Sum usage for each month based on the date range
        consumptionData.forEach((entry) => {
          const entryDate = new Date(entry.date);
          // Calculate the month index based on the difference between the entry date and the start date
          const monthIndex = (entryDate.getFullYear() - startDateObj.getFullYear()) * 12 + entryDate.getMonth() - startDateObj.getMonth();

          if (monthIndex >= 0 && monthIndex <= monthDifference) {
            monthlyConsumption[monthIndex] += entry.usage;
          }
        });

        // Transform data for the chart
        const realData = monthlyConsumption.map((usage, index) => {
          const month = (startDateObj.getMonth() + index) % 12;
          const year = startDateObj.getFullYear() + Math.floor((startDateObj.getMonth() + index) / 12);

          return {
            date: `${year}-${String(month + 1).padStart(2, "0")}`, // Format as YYYY-MM
            usage,
          };
        });

        setRealConsumptionData(realData);
      }
    } catch (error) {
      console.error("Error fetching real consumption data:", error);
    }
  };



  // Fetch predicted consumption data
  const fetchPredictedConsumptionData = async () => {
    try {
      const response = await axios.post("http://localhost:5154/api/Prediction/predict", null, {
        params: {
          consumptionType,
          buildingId: buildingId || "",
          months: numOfMonths,
        },
      });

      if (response.status === 200 && response.data && response.data.predictions) {
        const predictions = response.data.predictions.predictions.map((entry) => ({
          date: entry.date.split("T")[0],
          predictedUsage: entry.predicted_Usage,
        }));

        setPredictedData(predictions);
      }
    } catch (error) {
      console.error("Error fetching predicted consumption data:", error);
    }
  };

  const validateForm = () => {
    const errors = {};

    if (!numOfMonths || numOfMonths <= 0) {
      errors.numOfMonths = "Prediction months must be greater than 0.";
    }

    if (!startDate) {
      errors.startDate = "Start date is required.";
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      setValidated(false);
      return;
    }

    setValidated(true);
    fetchRealConsumptionData();
    fetchPredictedConsumptionData();
  };

  useEffect(() => {
    fetchElectricBuildings();
    fetchNaturalGasBuildings();
  }, []);

  useEffect(() => {
    if (consumptionType === "electric") {
      setBuildings(electricBuildings);
    } else if (consumptionType === "naturalgas") {
      setBuildings(gasBuildings);
    } else {
      setBuildings([]);
    }
  }, [consumptionType, electricBuildings, gasBuildings]);
  
// Find the last dated real consumption data
const lastRealDataPoint = realConsumptionData.length > 0
  ? realConsumptionData.reduce((latest, current) =>
      new Date(current.date) > new Date(latest.date) ? current : latest
    )
  : { date: "", usage: 0 };

// Adjust predicted data to start from the last real data point's date
const adjustedPredictedData = [
  {
    date: lastRealDataPoint.date,  // Start from the latest real data date
    usage: lastRealDataPoint.usage,
  },
  ...predictedData.map((item) => ({
    date: item.date,
    usage: item.predictedUsage,
  })),
];

// Merge and sort data for the chart
const mergedData = [
  ...realConsumptionData.map(item => ({
    date: item.date,
    usage: item.usage,
    type: 'real',
  })),
  ...adjustedPredictedData.map(item => ({
    date: item.date,
    usage: item.usage,
    type: 'predicted',
  })),
].sort((a, b) => new Date(a.date) - new Date(b.date));

// Chart labels
const labels = mergedData.map(item => item.date);

// Map data for real and predicted datasets
const realConsumptionDataMapped = labels.map(label => {
  const item = mergedData.find(d => d.date === label && d.type === 'real');
  return item ? item.usage : null;
});

const predictedConsumptionDataMapped = labels.map(label => {
  const item = mergedData.find(d => d.date === label && d.type === 'predicted');
  return item ? item.usage : null;
});

// Final chart data
const chartData = {
  labels: labels,
  datasets: [
    {
      label: "Real Consumption",
      data: realConsumptionDataMapped,
      borderColor: "#36A2EB",
      fill: false,
      tension: 0.1,
    },
    {
      label: "Predicted Consumption",
      data: predictedConsumptionDataMapped,
      borderColor: "#FF5733",
      fill: false,
      tension: 0.1,
    },
  ],
};

  return (
    <CRow>
      <CCol>
        <CCard className="mb-4">
          <CCardHeader>
            <h6>Prediction Results</h6>
          </CCardHeader>
          <CCardBody>
            <CForm noValidate onSubmit={handleSubmit}>
              <CRow className="mb-3">
                <CCol xs="12" md="2">
                  <CFormLabel>Consumption Type</CFormLabel>
                  <CFormSelect
                    value={consumptionType}
                    onChange={(e) => setConsumptionType(e.target.value)}
                  >
                    <option value="water">Water</option>
                    <option value="electric">Electricity</option>
                    <option value="naturalgas">Natural Gas</option>
                  </CFormSelect>
                </CCol>

                <CCol xs="12" md="4">
                  <CFormLabel>Building</CFormLabel>
                  <CFormSelect
                    value={buildingId}
                    onChange={(e) => setBuildingId(e.target.value)}
                    disabled={consumptionType === "water"}
                  >
                    <option value="">All Buildings</option>
                    {buildings.map((building) => (
                      <option key={building.id} value={building.id}>
                        {building.label}
                      </option>
                    ))}
                  </CFormSelect>
                </CCol>

                <CCol xs="12" md="2">
                  <CFormLabel>Prediction Months</CFormLabel>
                  <CInputGroup>
                    <CFormInput
                      type="number"
                      value={numOfMonths}
                      onChange={(e) => setNumOfMonths(e.target.value)}
                      isInvalid={!!formErrors.numOfMonths}
                    />
                    <CFormFeedback invalid>{formErrors.numOfMonths}</CFormFeedback>
                  </CInputGroup>
                </CCol>
                <CCol xs="12" md="3">
                  <CFormLabel>Start Date (Past Data)</CFormLabel>
                  <CFormInput
                    type="date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    isInvalid={!!formErrors.startDate}
                  />
                  <CFormFeedback invalid>{formErrors.startDate}</CFormFeedback>
                </CCol>
              </CRow>


              <CButton type="submit" color="primary">
                Submit
              </CButton>
            </CForm>

            <CRow className="mt-4">
              <CCol>
                <CChartLine
                  data={chartData}
                  options={{
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                      y: {
                        beginAtZero: true,  // Y-axis starts from 0
                        min: 0,
                      },
                    },
                  }}
                  style={{ height: "400px" }}
                />

              </CCol>
            </CRow>

            <CRow className="mt-4">
              <CCol>
                <CChartBar
                  data={{
                    labels: predictedData.map((item) => item.date),
                    datasets: [
                      {
                        label: "Predicted Consumption",
                        backgroundColor: "#FF5733",
                        data: predictedData.map((item) => item.predictedUsage),
                      },
                    ],
                  }}
                  options={{ responsive: true, maintainAspectRatio: false }}
                  style={{ height: "400px" }}
                />
              </CCol>
            </CRow>
          </CCardBody>
        </CCard>
      </CCol>
    </CRow>
  );
};

export default PredictionResult;
