import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
    CRow,
    CCol,
    CFormSelect,
    CFormLabel,
    CFormInput,
    CCard,
    CCardHeader,
    CCardBody,
    CTable,
    CTableHead,
    CTableRow,
    CTableHeaderCell,
    CTableBody,
    CTableDataCell,
} from '@coreui/react';
import { CChartLine } from '@coreui/react-chartjs';

const AnomaliesDetectionPage = () => {
    const [barChartSelectedBuilding, setBarChartSelectedBuilding] = useState(null);
    const [consumptionType, setConsumptionType] = useState('electric'); // Default: Electric
    const [threshold, setThreshold] = useState(0.2);
    const [buildings, setBuildings] = useState([]);
    const [data, setData] = useState([]);
    const [anomalies, setAnomalies] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchBuildings = async () => {
        try {
            const response = await axios.get("http://localhost:5154/api/Building");
            const filteredBuildings = response.data.buildings
                .filter((building) =>
                    consumptionType === 'electric'
                        ? building.e_MeterCode !== null
                        : building.g_MeterCode !== null
                )
                .map((building) => ({
                    id: building.id,
                    label: building.name,
                    value:
                        consumptionType === 'electric'
                            ? building.e_MeterCode
                            : building.g_MeterCode,
                }));
            setBuildings(filteredBuildings);
        } catch (error) {
            console.error("Error fetching buildings:", error);
        }
    };

    const fetchData = async () => {
        if (consumptionType !== 'water' && !barChartSelectedBuilding) return;

        const endpoint =
            consumptionType === 'electric'
                ? `http://localhost:5154/api/Electric?buildingId=${barChartSelectedBuilding.id}`
                : consumptionType === 'naturalgas'
                    ? `http://localhost:5154/api/NaturalGas?buildingId=${barChartSelectedBuilding.id}`
                    : `http://localhost:5154/api/Water`; // New Water Endpoint

        setLoading(true);
        setError(null);

        try {
            const response = await axios.get(endpoint);
            const usageData =
                consumptionType === 'electric'
                    ? response.data.electrics
                    : consumptionType === 'naturalgas'
                        ? response.data.naturalGas
                        : response.data.waters; // Water data

            if (!Array.isArray(usageData)) {
                throw new Error('Invalid data format. Expected an array.');
            }

            const formattedData = usageData.map((item) => ({
                ...item,
                usage: parseFloat(item.usage),
            }));

            setData(formattedData);
        } catch (error) {
            console.error('Error fetching data:', error);
            setError('Failed to load usage data.');
        } finally {
            setLoading(false);
        }
    };


    const fetchAnomalies = async () => {
        // Skip buildingId for water, require it for electric/naturalgas
        if (consumptionType !== 'water' && !barChartSelectedBuilding) return;

        const payload =
            consumptionType === 'water'
                ? { consumptionType, threshold }  // No buildingId for water
                : { consumptionType, buildingId: barChartSelectedBuilding.id, threshold };

        setLoading(true);
        setError(null);

        try {
            const response = await axios.post(
                "http://localhost:5154/api/Anomaly/get-anomaly",
                null,
                { params: payload }
            );

            const fetchedAnomalies = response.data.anomalies?.anomalies || [];
            setAnomalies(fetchedAnomalies);
        } catch (error) {
            if(error.response?.data?.error)
            {
                setAnomalies([]);
                console.log('No anomalies detected.');
                return;
            }
            if (error.response?.data?.error === 'Failed to get anomalies: { "error": "Not enough data to detect anomalies. Minimum required is 10 rows, but got 0." }') {
                setAnomalies([]);
                console.log('No anomalies detected.');
                return;
            }
            console.error('Error fetching anomalies:', error);
            setError('Failed to fetch anomalies.');
        } finally {
            setLoading(false);
        }
    };


    useEffect(() => {
        if (consumptionType === 'water') {
            fetchData();
            fetchAnomalies();
        }
        else {
            fetchBuildings();
        }
    }, [consumptionType]);

    useEffect(() => {
        if (barChartSelectedBuilding) {
            fetchData();
            fetchAnomalies();
        }
    }, [barChartSelectedBuilding, consumptionType]);


    useEffect(() => {
        if (barChartSelectedBuilding || consumptionType === 'water') {
            fetchAnomalies();
        }
    }, [threshold]);

    return (
        <div>
            <CRow className="mb-3">
                <CCol xs={12} md={6}>
                    <h3>Anomalies Detection</h3>
                </CCol>
                <CCol xs={6} md={6}>
                    <CFormLabel>Consumption Type</CFormLabel>
                    <CFormSelect
                        value={consumptionType}
                        onChange={(e) => setConsumptionType(e.target.value)}
                    >
                        <option value="electric">Electricity</option>
                        <option value="naturalgas">Natural Gas</option>
                        <option value="water">Water</option>
                    </CFormSelect>
                </CCol>
            </CRow>

            <CRow className="mb-3">
                <CCol xs={6} md={6}>
                    <CFormLabel>Threshold</CFormLabel>
                    <CFormInput
                        type="number"
                        step="0.01"
                        value={threshold}
                        onChange={(e) => setThreshold(parseFloat(e.target.value))}
                    />
                </CCol>

                {consumptionType !== 'water' && (
                    <CCol xs={6} md={6}>
                        <CFormLabel>Select Building</CFormLabel>
                        <CFormSelect
                            value={barChartSelectedBuilding?.value || ''}
                            onChange={(e) => {
                                const selectedBuilding = buildings.find(
                                    (b) => b.value === e.target.value
                                );
                                setBarChartSelectedBuilding(selectedBuilding);
                            }}
                        >
                            <option value="" disabled>
                                Select a building
                            </option>
                            {buildings.map((building) => (
                                <option key={building.id} value={building.value}>
                                    {building.label}
                                </option>
                            ))}
                        </CFormSelect>
                    </CCol>
                )}

            </CRow>

            {loading ? (
                <p>Loading data...</p>
            ) : error ? (
                <p className="text-danger">{error}</p>
            ) : (
                <>
                    <CCard className="mb-4">
                        <CCardHeader> Normal vs Anomalies</CCardHeader>
                        <CCardBody>
                            <CChartLine
                                data={{
                                    labels: data.map((item) => {
                                        const isAnomaly = anomalies.some(
                                            (anomaly) => anomaly.date.slice(0, 10) === item.date.slice(0, 10)
                                        );
                                        return isAnomaly
                                            ? `${item.date.slice(0, 7)} `
                                            : item.date.slice(0, 7);
                                    }),
                                    datasets: [
                                        {
                                            label: 'Normal Usage',
                                            borderColor: '#36A2EB',
                                            backgroundColor: '#36A2EB',
                                            pointRadius: 3,
                                            pointHoverRadius: 5,
                                            data: data.map((item) => item.usage),
                                            fill: false,
                                            tension: 0,
                                        },
                                        {
                                            label: 'Anomalies',
                                            borderColor: '#FF6384',
                                            backgroundColor: '#FF6384',
                                            pointRadius: 5,
                                            pointHoverRadius: 7,
                                            data: data.map((item) => {
                                                const isAnomaly = anomalies.some(
                                                    (anomaly) => anomaly.date.slice(0, 10) === item.date.slice(0, 10)
                                                );
                                                return isAnomaly ? item.usage : null;
                                            }),
                                            fill: false,
                                            tension: 0,
                                        },
                                    ],
                                }}
                                options={{
                                    responsive: true,
                                    maintainAspectRatio: false,
                                    plugins: {
                                        tooltip: {
                                            callbacks: {
                                                label: function (context) {
                                                    const index = context.dataIndex;
                                                    const isAnomaly = anomalies.some(
                                                        (anomaly) =>
                                                            anomaly.date.slice(0, 10) === data[index].date.slice(0, 10)
                                                    );
                                                    return isAnomaly
                                                        ? `Usage: ${context.raw} (Anomaly)`
                                                        : `Usage: ${context.raw}`;
                                                },
                                            },
                                        },
                                    },
                                }}
                            />

                        </CCardBody>
                    </CCard>

                    { anomalies.length > 0 ?
                        (<CCard>
                            <CCardHeader>Anomalies</CCardHeader>
                            <CCardBody>
                                <CTable striped hover responsive>
                                    <CTableHead>
                                        <CTableRow>
                                            <CTableHeaderCell>Date</CTableHeaderCell>
                                            <CTableHeaderCell>Usage</CTableHeaderCell>
                                            <CTableHeaderCell>Anomaly Error</CTableHeaderCell>
                                        </CTableRow>
                                    </CTableHead>
                                    <CTableBody>
                                        {anomalies.map((item, index) => {
                                            const matchingUsage = data.find(
                                                (usageItem) => usageItem.date.slice(0, 10) === item.date.slice(0, 10)
                                            );

                                            return (
                                                <CTableRow key={index} color='danger'>
                                                    <CTableDataCell>{item.date?.slice(0, 10)}</CTableDataCell>
                                                    <CTableDataCell>
                                                        {matchingUsage ? matchingUsage.usage : 'N/A'}
                                                    </CTableDataCell>
                                                    <CTableDataCell>
                                                        {item.anomaly_Error !== undefined
                                                            ? item.anomaly_Error.toFixed(4)
                                                            : '-'}
                                                    </CTableDataCell>
                                                </CTableRow>
                                            );
                                        })}
                                    </CTableBody>
                                </CTable>
                            </CCardBody>
                        </CCard> )
                        : 
                        (<CCard>
                            <CCardHeader>Anomalies</CCardHeader>
                            <CCardBody>
                                <p>No anomalies detected.</p>
                            </CCardBody>
                        </CCard>)
                        
                        }

                </>
            )}
        </div>
    );
};

export default AnomaliesDetectionPage;
