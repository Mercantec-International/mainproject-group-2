<!-- comparisons.php -->
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

<title>VitalMetrics - Comparisons</title>

<div class="bg-gray-800 text-gray-300 p-8 rounded-lg shadow-lg max-w-3xl mx-auto mt-8 mb-16">
<h1 class="text-3xl font-bold text-white mb-4">Comparisons</h1>
    <p>Here are the comparisons of the data:</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
    <p class="mb-8">Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa officiis beatae optio non unde veritatis incidunt temporibus.</p>
</div>

<?php include 'inc/footer.php'; ?>