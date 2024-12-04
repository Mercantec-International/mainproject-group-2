<!-- register.php -->
<?php include 'inc/header.php'; ?>
<title>Register - VitalMetrics</title>

<div class="bg-gray-800 text-gray-300 p-8 rounded-lg shadow-lg mx-auto w-[80%] mt-8">
    <h1 class="text-3xl font-bold text-white mb-6">Register</h1>
    <form id="registerForm" class="space-y-4">
        <!-- First Name -->
        <div>
            <label for="firstName" class="block text-sm font-medium text-gray-200">First Name</label>
            <input type="text" id="firstName" name="FirstName" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Last Name -->
        <div>
            <label for="lastName" class="block text-sm font-medium text-gray-200">Last Name</label>
            <input type="text" id="lastName" name="LastName" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Email -->
        <div>
            <label for="email" class="block text-sm font-medium text-gray-200">Email</label>
            <input type="email" id="email" name="Email" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Password -->
        <div>
            <label for="password" class="block text-sm font-medium text-gray-200">Password</label>
            <input type="password" id="password" name="Password" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Address -->
        <div>
            <label for="address" class="block text-sm font-medium text-gray-200">Address</label>
            <input type="text" id="address" name="Address" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- City -->
        <div>
            <label for="city" class="block text-sm font-medium text-gray-200">City</label>
            <input type="text" id="city" name="City" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Region -->
        <div>
            <label for="region" class="block text-sm font-medium text-gray-200">Region</label>
            <input type="text" id="region" name="Region" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Postal Code -->
        <div>
            <label for="postal" class="block text-sm font-medium text-gray-200">Postal Code</label>
            <input type="text" id="postal" name="Postal" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Country -->
        <div>
            <label for="country" class="block text-sm font-medium text-gray-200">Country</label>
            <input type="text" id="country" name="Country" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Phone -->
        <div>
            <label for="phone" class="block text-sm font-medium text-gray-200">Phone</label>
            <input type="text" id="phone" name="Phone" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Username -->
        <div>
            <label for="username" class="block text-sm font-medium text-gray-200">Username</label>
            <input type="text" id="username" name="Username" class="mt-1 block w-full bg-gray-900 text-gray-300 border-gray-600 rounded-lg p-2" required>
        </div>

        <!-- Register Button -->
        <button type="submit" class="w-full bg-blue-500 text-white p-3 rounded-lg hover:bg-blue-600">Register</button>
    </form>
</div>

<script>
async function registerUser(event) {
    event.preventDefault(); // Prevent form from refreshing the page
    const form = document.getElementById('registerForm');
    const formData = new FormData(form);

    try {
        const response = await fetch('https://mainproject-group-2.onrender.com/api/User/SignUp', {
            method: 'POST',
            body: formData,
        });

        if (response.ok) {
            alert('Registration successful!');
            window.location.href = 'index.php'; // Redirect to home or login page
        } else {
            const errorData = await response.json();
            console.error('Error:', errorData);
            alert('Error: ' + (errorData.message || 'Unable to register.'));
        }
    } catch (error) {
        console.error('Network error:', error);
        alert('Network error: Unable to connect to the server.');
    }
}

// Attach the function to the form's submit event
document.getElementById('registerForm').addEventListener('submit', registerUser);
</script>

<?php include 'inc/footer.php'; ?>