<?php
// Start the session and destroy it
session_start();
session_unset();
session_destroy();
?>
<script>
    // Clear JWT token and login status from localStorage
    localStorage.removeItem('jwt'); // Remove the JWT token
    localStorage.removeItem('loggedin'); // Remove the loggedin flag

    // Redirect the user to the homepage
    window.location.href = 'index.php';
</script>
