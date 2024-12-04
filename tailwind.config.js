/** @type {import('tailwindcss').Config} */
// tailwind.config.js
module.exports = {
  content: [
    './**/*.php', // Include all PHP files
    './**/*.html', // Include HTML files if applicable
    './**/*.js',   // Include JavaScript files if needed
  ],
  theme: {
    extend: {
      // Customize your theme here if needed
      colors: {
        primary: '#1f1f1f', // Example custom colors
        secondary: '#121212',
        accent: '#90caf9',
      },
    },
  },
  plugins: [],
}