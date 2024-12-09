<?php include 'inc/header.php'; ?>

<script>
document.addEventListener('DOMContentLoaded', () => {
    // Check if the user is logged in by verifying the "loggedin" flag in localStorage
    const isLoggedIn = localStorage.getItem('loggedin') === 'true';

    if (!isLoggedIn) {
        // If not logged in, redirect to the login page
        alert('You are not logged in. Redirecting to login...');
        window.location.href = 'login.php';
    }
});
</script>

<title>VitalMetrics - Results</title>

<div class="bg-gray-800 text-gray-300 p-8 rounded-lg shadow-lg w-[80%] mx-auto mt-8 mb-16">
    <h1 class="text-3xl font-bold text-white mb-4">Results</h1>
    <p class="mb-6">Explore the oximeter, heartbeat, and accelerometer stats below:</p>

    <!-- Accelerometer Stats Section -->
    <div class="mb-12">
        <h2 class="text-2xl font-semibold text-white mb-4">Accelerometer Stats</h2>

        <!-- Accelerometer Record by ID -->
        <div>
            <h3 class="text-xl font-semibold text-white mb-4">Get Accelerometer Record by ID</h3>
            <form class="space-y-4">
                <div>
                    <label for="accelerometerRecordId" class="block text-sm font-medium text-gray-200">Enter ID</label>
                    <input type="text" id="accelerometerRecordId" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
                </div>
                <button type="button" onclick="fetchAccelerometerById()" class="bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Fetch Accelerometer Record</button>
            </form>
            <div id="accelerometerById" class="mt-4 bg-gray-900 p-4 rounded-lg shadow-md hidden"></div>
        </div>
    </div>
</div>

<script>
// Function to fetch Accelerometer Data by ID
async function fetchAccelerometerById() {
    const id = document.getElementById('accelerometerRecordId').value;
    const isLoggedIn = localStorage.getItem('loggedin') === 'true';

    if (!isLoggedIn) {
        alert('You are not logged in. Redirecting to login...');
        window.location.href = 'login.php';
        return;
    }

    const token = localStorage.getItem('jwt'); // JWT is still needed for API authorization

    try {
        const response = await fetch(`https://mainproject-group-2.onrender.com/api/Accelerometer/getaccbyid/${id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`, // Use the JWT token for API calls
            },
        });

        if (response.ok) {
            const data = await response.json();
            displayAccelerometerData(data);
        } else {
            const error = await response.json();
            alert(`Error: ${error.message || 'Unable to fetch data'}`);
        }
    } catch (error) {
        console.error('Network error:', error);
        alert('Network error: Unable to fetch data.');
    }
}

// Function to display accelerometer data
function displayAccelerometerData(data) {
    const container = document.getElementById('accelerometerById');
    if (data) {
        const tableHTML = `
            <table class="table-auto w-full text-gray-300">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>X</th>
                        <th>Y</th>
                        <th>Z</th>
                        <th>Changes</th>
                        <th>Created At</th>
                        <th>Updated At</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>${data.id}</td>
                        <td>${data.x}</td>
                        <td>${data.y}</td>
                        <td>${data.z}</td>
                        <td>${data.changes}</td>
                        <td>${new Date(data.createdAt).toLocaleString()}</td>
                        <td>${new Date(data.updatedAt).toLocaleString()}</td>
                    </tr>
                </tbody>
            </table>
        `;
        container.innerHTML = tableHTML;
        container.classList.remove('hidden');
    } else {
        container.innerHTML = '<p class="text-red-500">No data found for the given ID.</p>';
        container.classList.remove('hidden');
    }
}
</script>

<?php include 'inc/footer.php'; ?>
