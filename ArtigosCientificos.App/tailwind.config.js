/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Views/**/*.cshtml',   // Include Razor Views
        './wwwroot/**/*.html',   // Include any HTML files in the static folder
    ],
    theme: {
        extend: {},
    },
    plugins: [],
};

