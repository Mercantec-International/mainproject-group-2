<?php include 'inc/header.php'; ?>
<title>VitalMetrics - Results</title>

<div class="bg-gray-800 text-gray-300 p-8 rounded-lg shadow-lg w-[80%] mx-auto mt-8 mb-16">
    <h1 class="text-3xl font-bold text-white mb-4">Results</h1>
    <p class="mb-6">Explore the oximeter and heartbeat stats below:</p>

    <!-- Oximeter Stats Section -->
    <div class="mb-12">
        <h2 class="text-2xl font-semibold text-white mb-4">Oximeter Stats</h2>

        <!-- All Oximeter Records -->
        <div class="mb-8">
            <h3 class="text-xl font-semibold text-white mb-4">All Oximeter Records</h3>
            <button onclick="fetchAllOximeterData()" class="bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Load All Oximeter Data</button>
            <div id="allOximeterData" class="mt-4 bg-gray-900 p-4 rounded-lg shadow-md overflow-x-auto hidden"></div>
            <div id="oximeterLoader" class="mt-4 hidden text-center text-white">Loading...</div>
        </div>

        <!-- Oximeter Record by ID -->
        <div>
            <h3 class="text-xl font-semibold text-white mb-4">Get Oximeter Record by ID</h3>
            <form class="space-y-4">
                <div>
                    <label for="oximeterRecordId" class="block text-sm font-medium text-gray-200">Enter ID</label>
                    <input type="text" id="oximeterRecordId" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
                </div>
                <button type="button" onclick="fetchOximeterById()" class="bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Fetch Oximeter Record</button>
            </form>
            <div id="oximeterById" class="mt-4 bg-gray-900 p-4 rounded-lg shadow-md hidden"></div>
        </div>
    </div>

    <!-- Heartbeat Stats Section -->
    <div>
        <h2 class="text-2xl font-semibold text-white mb-4">Heartbeat Stats</h2>

        <!-- All Heartbeat Records -->
        <div class="mb-8">
            <h3 class="text-xl font-semibold text-white mb-4">All Heartbeat Records</h3>
            <button onclick="fetchAllHeartbeatData()" class="bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Load All Heartbeat Data</button>
            <div id="allHeartbeatData" class="mt-4 bg-gray-900 p-4 rounded-lg shadow-md overflow-x-auto hidden"></div>
            <div id="heartbeatLoader" class="mt-4 hidden text-center text-white">Loading...</div>
        </div>

        <!-- Heartbeat Record by ID -->
        <div>
            <h3 class="text-xl font-semibold text-white mb-4">Get Heartbeat Record by ID</h3>
            <form class="space-y-4">
                <div>
                    <label for="heartbeatRecordId" class="block text-sm font-medium text-gray-200">Enter ID</label>
                    <input type="text" id="heartbeatRecordId" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
                </div>
                <button type="button" onclick="fetchHeartbeatById()" class="bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Fetch Heartbeat Record</button>
            </form>
            <div id="heartbeatById" class="mt-4 bg-gray-900 p-4 rounded-lg shadow-md hidden"></div>
        </div>
    </div>
</div>

<script>
// Function to display records in a table format
function displayRecords(data, containerId) {
    const container = document.getElementById(containerId);
    if (Array.isArray(data) && data.length) {
        let tableHTML = `<table class="table-auto w-full text-gray-300"><thead><tr><th>ID</th><th>Value</th><th>Timestamp</th></tr></thead><tbody>`;
        data.forEach(item => {
            tableHTML += `<tr><td>${item.id || item._id}</td><td>${item.value || item.bpm}</td><td>${item.timestamp || item.createdAt}</td></tr>`;
        });
        tableHTML += `</tbody></table>`;
        container.innerHTML = tableHTML;
    } else {
        container.innerHTML = '<p class="text-red-500">No records found.</p>';
    }
    container.classList.remove('hidden');
}

// Fetch all Oximeter Data
async function fetchAllOximeterData() {
    const loader = document.getElementById('oximeterLoader');
    const container = document.getElementById('allOximeterData');
    loader.classList.remove('hidden');
    try {
        const response = await fetch('https://mainproject-group-2.onrender.com/api/Oxilevel/getall');
        if (response.ok) {
            const data = await response.json();
            displayRecords(data, 'allOximeterData');
        } else {
            alert('Failed to load oximeter data.');
        }
    } catch (error) {
        console.error('Error fetching oximeter data:', error);
        alert('Error fetching data.');
    }
    loader.classList.add('hidden');
}

// Fetch Oximeter Data by ID
async function fetchOximeterById() {
    const id = document.getElementById('oximeterRecordId').value;
    const container = document.getElementById('oximeterById');
    if (!id) {
        alert('Please enter an ID.');
        return;
    }

    try {
        const response = await fetch(`https://mainproject-group-2.onrender.com/api/Oxilevel/getall/${id}`);
        if (response.ok) {
            const data = await response.json();
            displayRecords([data], 'oximeterById');
        } else {
            alert('Failed to fetch oximeter record.');
        }
    } catch (error) {
        console.error('Error fetching oximeter record:', error);
        alert('Error fetching record.');
    }
}

// Fetch all Heartbeat Data
async function fetchAllHeartbeatData() {
    const loader = document.getElementById('heartbeatLoader');
    const container = document.getElementById('allHeartbeatData');
    loader.classList.remove('hidden');
    try {
        const response = await fetch('https://mainproject-group-2.onrender.com/api/Earheartbeat/getall');
        if (response.ok) {
            const data = await response.json();
            displayRecords(data, 'allHeartbeatData');
        } else {
            alert('Failed to load heartbeat data.');
        }
    } catch (error) {
        console.error('Error fetching heartbeat data:', error);
        alert('Error fetching data.');
    }
    loader.classList.add('hidden');
}

// Fetch Heartbeat Data by ID
async function fetchHeartbeatById() {
    const id = document.getElementById('heartbeatRecordId').value;
    const container = document.getElementById('heartbeatById');
    if (!id) {
        alert('Please enter an ID.');
        return;
    }

    try {
        const response = await fetch(`https://mainproject-group-2.onrender.com/api/Earheartbeat/${id}`);
        if (response.ok) {
            const data = await response.json();
            displayRecords([data], 'heartbeatById');
        } else {
            alert('Failed to fetch heartbeat record.');
        }
    } catch (error) {
        console.error('Error fetching heartbeat record:', error);
        alert('Error fetching record.');
    }
}
</script>

<?php include 'inc/footer.php'; ?>