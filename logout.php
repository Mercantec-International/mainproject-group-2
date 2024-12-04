<?php
session_start();
session_unset(); // Remove session variables
session_destroy(); // Destroy the session
?>
<script>
    // Clear localStorage login status
    localStorage.removeItem('loggedin');
    // Redirect to homepage
    window.location.href = 'index.php';
</script>