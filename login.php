<?php
// Start the session to manage login status
session_start();
?>

<!-- login.php -->
<?php include 'inc/header.php'; ?>
<title>Login - VitalMetrics</title>

<div class="bg-gray-800 text-gray-300 p-8 rounded-lg shadow-lg mx-auto w-[80%] mt-8">
    <h1 class="text-3xl font-bold text-white mb-6">Login</h1>
    <form id="loginForm" class="space-y-4">
        <!-- Email -->
        <div>
            <label for="email" class="block text-sm font-medium text-gray-200">Email</label>
            <input type="email" id="email" name="email" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Password -->
        <div>
            <label for="password" class="block text-sm font-medium text-gray-200">Password</label>
            <input type="password" id="password" name="password" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Login Button -->
        <button type="button" onclick="loginUser()" class="w-full bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Login</button>

        <p class="mt-4 text-sm text-gray-400">Please log into VitalMetrics to see your data.</p>
    </form>
</div>

<script>
async function loginUser() {
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    // Basic validation
    if (!email || !password) {
        alert('Please enter both email and password.');
        return;
    }

    const requestBody = { email, password };

    try {
        const response = await fetch('https://mainproject-group-2.onrender.com/api/User/Login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestBody),
        });

        if (response.ok) {
            const data = await response.json();
            const token = data.token; // Assuming the response contains a JWT token

            // Store the JWT token and login status in localStorage
            localStorage.setItem('jwt', token);
            localStorage.setItem('loggedin', true);

            alert('Login successful!');
            window.location.href = 'index.php'; // Redirect to the homepage/dashboard
        } else {
            const error = await response.json();
            alert(`Error: ${error.message || 'Invalid credentials'}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Network error: Unable to connect to the server.');
    }
}
</script>

<?php include 'inc/footer.php'; ?>