<?php
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="icon" href="img/logo.png" type="image/png">
    <link rel="stylesheet" href="public/css/styles.css">
    <title>VitalMetrics</title>
</head>
<body>

<nav class="navbar bg-gray-900 text-gray-200 p-4 flex justify-between items-center">
    <div class="flex items-center space-x-2">
        <img src="img/logo.png" alt="VitalMetrics Logo" class="h-6 w-auto">
        <a href="index.php" class="text-white text-lg font-semibold">VitalMetrics</a>
    </div>
    <div class="space-x-4">
        <!-- Links always visible -->
        <a href="index.php" class="hover:text-blue-400 transition duration-200">Home</a>
        <a href="about.php" class="hover:text-blue-400 transition duration-200">About Us</a>
        <a href="product.php" class="hover:text-blue-400 transition duration-200">Product</a>

        <!-- Conditional Links for Logged In Users -->
        <a href="results.php" id="resultsLink" class="hover:text-blue-400 transition duration-200" style="display:none;">Results</a>
        <a href="comparison.php" id="comparisonLink" class="hover:text-blue-400 transition duration-200" style="display:none;">Comparisons</a>

        <!-- Conditional Login/Register/Logout -->
        <a href="login.php" id="loginBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold">Login</a>
        <a href="register.php" id="registerBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold">Register</a>
        <a href="#" id="logoutBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold" style="display:none;">Logout</a>
    </div>
</nav>

<script>
// Check login status using localStorage
const isLoggedIn = localStorage.getItem('loggedin') === 'true';

// Handle visibility of links and buttons
if (isLoggedIn) {
    // User is logged in
    document.getElementById('loginBtn').style.display = 'none';
    document.getElementById('registerBtn').style.display = 'none';
    document.getElementById('logoutBtn').style.display = 'inline-block';

    // Show "Results" and "Comparisons" links
    document.getElementById('resultsLink').style.display = 'inline-block';
    document.getElementById('comparisonLink').style.display = 'inline-block';
} else {
    // User is not logged in
    document.getElementById('loginBtn').style.display = 'inline-block';
    document.getElementById('registerBtn').style.display = 'inline-block';
    document.getElementById('logoutBtn').style.display = 'none';

    // Hide "Results" and "Comparisons" links
    document.getElementById('resultsLink').style.display = 'none';
    document.getElementById('comparisonLink').style.display = 'none';
}

// Logout button functionality
document.getElementById('logoutBtn').addEventListener('click', function() {
    // Clear login status from localStorage
    localStorage.removeItem('loggedin');

    // Redirect to homepage or login page
    window.location.href = 'index.php';
});
</script>

</body>
</html>