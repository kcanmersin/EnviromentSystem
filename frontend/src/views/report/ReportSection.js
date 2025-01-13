import { CAlert, CButton, CRow, CCol, CSpinner } from '@coreui/react';
import axios from 'axios';
import React, { useState } from 'react';

const ReportSection = ({ type }) => {
    const [downloadingReport, setDownloadingReport] = useState(false); // State to track downloading

    // Define the color and label based on the 'type' prop
    const typeData = {
        naturalGas: { color: 'info', label: 'Natural Gas Report', value: 'NaturalGas' },
        electric: { color: 'primary', label: 'Electric Report', value: 'Electric' },
        water: { color: 'warning', label: 'Water Report', value: 'Water' },
        paper: { color: 'success', label: 'Paper Report', value: 'Paper' },
    };


    const downloadReport = async () => {

        setDownloadingReport(true);

        try {
            const response = await axios.get('http://localhost:5154/api/Consumption/export', {
                params: {
                    includeGraphs: true,
                    consumptionType: typeData[type].value, // Assuming typeData is defined and has the correct value
                },
                responseType: 'blob', // To handle Excel file download
            });

            // Create a link to trigger the download
            const fileURL = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = fileURL;
            const fileName = `${typeData[type].value}_Report.xlsx`;
            link.setAttribute('download', fileName); // File name
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } catch (error) {
            console.error('Error downloading report:', error);
        } finally {
            setDownloadingReport(false);
        }
    };

    const reportData = typeData[type] || typeData.naturalGas; // Default to naturalGas if type is invalid

    return (
        <CRow className="mb-4">
            <CCol>
                {/* Alert Box for the message */}
                <CAlert color={reportData.color} className="d-flex justify-content-between align-items-center">
                    <span className="text-dark">
                        <strong>{reportData.label}: Check out the new feature!</strong>
                    </span>

                    {/* Download Button */}
                    <CButton
                        color="warning"
                        onClick={downloadReport} // Assuming downloadReport function is defined
                        disabled={downloadingReport} // Assuming downloadingReport state is defined
                        className="d-flex align-items-center"
                    >
                        {downloadingReport ? (
                            <CSpinner size="sm" className="me-2" />
                        ) : (
                            <i className="cil-cloud-download me-2"></i> // Add an icon for the button
                        )}
                        {downloadingReport ? 'Generating Report...' : 'Download Report'}
                    </CButton>
                </CAlert>
            </CCol>
        </CRow>
    );
};

export default ReportSection;
