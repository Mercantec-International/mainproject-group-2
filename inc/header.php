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
</head>
<body>

<nav class="navbar bg-gray-900 text-gray-200 p-4 flex justify-between items-center">
    <div class="flex items-center space-x-2">
        <img src="img/logo.png" alt="VitalMetrics Logo" class="h-6 w-auto">
        <a href="index.php" class="text-white text-lg font-semibold">VitalMetrics</a>
    </div>
    <div class="space-x-4">
        <a href="index.php" class="hover:text-blue-400 transition duration-200">Home</a>
        <a href="about.php" class="hover:text-blue-400 transition duration-200">About Us</a>
        <a href="product.php" class="hover:text-blue-400 transition duration-200">Product</a>
        <a href="results.php" class="hover:text-blue-400 transition duration-200">Result</a>
        <a href="comparison.php" class="hover:text-blue-400 transition duration-200">Comparisons</a>

        <!-- Conditional Login/Register/Logout -->
        <a href="login.php" id="loginBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold">Login</a>
        <a href="register.php" id="registerBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold">Register</a>
        <a href="logout.php" id="logoutBtn" class="hover:text-blue-400 transition duration-200 text-lg font-semibold" style="display:none;">Logout</a>
    </div>
</nav>

<script>
// Check if the user is logged in (using localStorage)
const isLoggedIn = localStorage.getItem('loggedin');

// Show/hide buttons based on login status
if (isLoggedIn) {
    document.getElementById('loginBtn').style.display = 'none';
    document.getElementById('registerBtn').style.display = 'none';
    document.getElementById('logoutBtn').style.display = 'inline-block';
} else {
    document.getElementById('loginBtn').style.display = 'inline-block';
    document.getElementById('registerBtn').style.display = 'inline-block';
    document.getElementById('logoutBtn').style.display = 'none';
}
</script>

</body>
</html>