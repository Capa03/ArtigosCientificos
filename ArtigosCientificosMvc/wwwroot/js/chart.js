﻿function drawSmallChart(stats) {
    var ctx = document.getElementById('articleStatisticsChart').getContext('2d');
    console.log(stats);
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Articles', 'Views', 'Downloads', "Most Viwed"],
            datasets: [{
                data: [stats.totalArticles, stats.totalViews, stats.totalDownloads, stats.mostViewedCategoryCount],
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4CAF50']
            }]
        },
        options: {
            responsive: false,
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Article Statistics Overview'
                }
            }
        }
    });
}

function drawSmallChartSingle(stats) {
    var ctx = document.getElementById('articleStatisticsChartSingle').getContext('2d');

    console.log(stats);

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Articles', 'Views', 'Downloads'],
            datasets: [{
                data: [stats.totalArticles, stats.totalViews, stats.totalDownloads],
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
            }]
        },
        options: {
            responsive: false,
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Article Statistics Overview'
                }
            }
        }
    });
}



/*function drawArticleStatisticsChart(stats) {
    var ctx = document.getElementById('articleStatisticsChart').getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Total Articles', 'Total Views', 'Total Downloads', 'Most Viewed Category'],
            datasets: [{
                label: 'Statistics',
                data: [stats.totalArticles, stats.totalViews, stats.totalDownloads, stats.mostViewedCategoryCount],
                backgroundColor: ['#4CAF50', '#FF9800', '#03A9F4', '#E91E63']
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Article Statistics Overview'
                }
            }
        }
    });
}*/
