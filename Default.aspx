<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KumariCinemas._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="kc-dashboard-page">
        <asp:Label ID="lblDashboardMsg" runat="server" CssClass="kc-msg" />

        <section class="kc-dashboard-hero" id="dashboardHero" data-poster-root="/Content/images/movies/">
            <div class="kc-dashboard-hero__bg kc-dashboard-hero__bg--a is-active" style="background-image:url('/Content/images/movies/Se7en.jpeg');"></div>
            <div class="kc-dashboard-hero__bg kc-dashboard-hero__bg--b"></div>
            <div class="kc-dashboard-hero__shade"></div>
            <div class="kc-dashboard-hero__overlay">
                <p class="kc-dashboard-hero__tag">Cinema Booking System</p>
                <h1>Kumari Cinemas Dashboard</h1>
                <p class="kc-dashboard-hero__lead">Track bookings, watch occupancy, and manage every screen from one place.</p>
                <div class="kc-hero-badges">
                    <span class="kc-hero-badge"><i class="bi bi-camera-reels"></i> Movies: <asp:Literal ID="litHeroMovies" runat="server" /></span>
                    <span class="kc-hero-badge"><i class="bi bi-building"></i> Theaters: <asp:Literal ID="litHeroTheaters" runat="server" /></span>
                    <span class="kc-hero-badge"><i class="bi bi-percent"></i> Occupancy: <asp:Literal ID="litHeroOccupancy" runat="server" /></span>
                </div>
            </div>
        </section>

        <section class="mb-4">
            <h4 class="kc-section-title">Key Performance Snapshot</h4>
            <div class="row g-3">
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Total Movies</p>
                        <h3><asp:Literal ID="litTotalMovies" runat="server" /></h3>
                    </div>
                </div>
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Total Theaters</p>
                        <h3><asp:Literal ID="litTotalTheaters" runat="server" /></h3>
                    </div>
                </div>
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Paid Seats Sold</p>
                        <h3><asp:Literal ID="litTicketsSold" runat="server" /></h3>
                    </div>
                </div>
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Bookings for Today's Shows</p>
                        <h3><asp:Literal ID="litDailyBookings" runat="server" /></h3>
                    </div>
                </div>
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Top Movie (Paid Seats)</p>
                        <h3 class="kc-kpi-title"><asp:Literal ID="litPopularMovie" runat="server" /></h3>
                    </div>
                </div>
                <div class="col-6 col-md-4 col-xl-2">
                    <div class="kc-kpi-card">
                        <p>Occupancy Rate</p>
                        <h3><asp:Literal ID="litOccupancyRate" runat="server" /></h3>
                    </div>
                </div>
            </div>
        </section>

        <section class="mb-4">
            <h4 class="kc-section-title kc-section-title-gold">Cinema Insights</h4>
            <div class="row g-3">
                <div class="col-lg-8">
                    <div class="kc-chart-card">
                        <div class="kc-chart-card__head">
                            <h5>Bookings for Shows Trend (Last 7 Days)</h5>
                        </div>
                        <canvas id="bookingsTrendChart" height="120"></canvas>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="kc-chart-card">
                        <div class="kc-chart-card__head">
                            <h5>Paid Seats by Movie</h5>
                        </div>
                        <canvas id="ticketsByMovieChart" height="120"></canvas>
                    </div>
                </div>
                <div class="col-12">
                    <div class="kc-chart-card">
                        <div class="kc-chart-card__head">
                            <h5>Occupancy by City Hall</h5>
                        </div>
                        <canvas id="occupancyByTheaterChart" height="95"></canvas>
                    </div>
                </div>
            </div>
        </section>

        <section class="mb-4">
            <h4 class="kc-section-title">Quick Navigation</h4>
            <div class="row g-3">
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="UserDetails.aspx" class="kc-quick-link-card">
                        <i class="bi bi-people-fill"></i>
                        <div>
                            <h5>User Details</h5>
                            <p>Manage customer records.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="TheaterCityHallDetails.aspx" class="kc-quick-link-card">
                        <i class="bi bi-building"></i>
                        <div>
                            <h5>Theater / City Hall</h5>
                            <p>Update venues and halls.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="MovieDetails.aspx" class="kc-quick-link-card">
                        <i class="bi bi-camera-reels-fill"></i>
                        <div>
                            <h5>Movie Details</h5>
                            <p>Maintain movie records.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="ShowtimesDetails.aspx" class="kc-quick-link-card">
                        <i class="bi bi-clock-fill"></i>
                        <div>
                            <h5>Showtimes</h5>
                            <p>Track daily schedules.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="TicketDetails.aspx" class="kc-quick-link-card">
                        <i class="bi bi-ticket-perforated-fill"></i>
                        <div>
                            <h5>Ticket Details</h5>
                            <p>Handle ticket records.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="UserTicket.aspx" class="kc-quick-link-card">
                        <i class="bi bi-person-badge-fill"></i>
                        <div>
                            <h5>User Ticket</h5>
                            <p>Check customer ticket history.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="TheaterCityHallMovie.aspx" class="kc-quick-link-card">
                        <i class="bi bi-collection-play-fill"></i>
                        <div>
                            <h5>Theater Movie</h5>
                            <p>Review hall movie lineups.</p>
                        </div>
                    </a>
                </div>
                <div class="col-md-6 col-lg-4 col-xl-3">
                    <a href="MovieTheatherCityHallOccupancyPerformer.aspx" class="kc-quick-link-card">
                        <i class="bi bi-graph-up-arrow"></i>
                        <div>
                            <h5>Occupancy Report</h5>
                            <p>See top hall performance.</p>
                        </div>
                    </a>
                </div>
            </div>
        </section>

        <asp:HiddenField ID="hfHeroSlides" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfChartBookings" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfChartTickets" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfChartOccupancy" runat="server" ClientIDMode="Static" />
    </main>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        (function () {
            function parseField(id, fallback) {
                var el = document.getElementById(id);
                if (!el || !el.value) return fallback;
                try { return JSON.parse(el.value); } catch (e) { return fallback; }
            }

            function renderLineChart(canvasId, series, label) {
                var ctx = document.getElementById(canvasId);
                if (!ctx || typeof Chart === "undefined") return;

                var labels = series.labels || series.Labels || [];
                var values = series.values || series.Values || [];

                new Chart(ctx, {
                    type: "line",
                    data: {
                        labels: labels,
                        datasets: [{
                            label: label,
                            data: values,
                            borderColor: "#e50914",
                            backgroundColor: "rgba(229, 9, 20, 0.2)",
                            fill: true,
                            tension: 0.35,
                            pointRadius: 3
                        }]
                    },
                    options: {
                        plugins: { legend: { labels: { color: "#cccccc" } } },
                        scales: {
                            x: { ticks: { color: "#9a9a9a" }, grid: { color: "rgba(255,255,255,0.06)" } },
                            y: { ticks: { color: "#9a9a9a" }, grid: { color: "rgba(255,255,255,0.06)" }, beginAtZero: true }
                        }
                    }
                });
            }

            function renderBarChart(canvasId, series, color, horizontal) {
                var ctx = document.getElementById(canvasId);
                if (!ctx || typeof Chart === "undefined") return;

                var labels = series.labels || series.Labels || [];
                var values = series.values || series.Values || [];

                new Chart(ctx, {
                    type: "bar",
                    data: {
                        labels: labels,
                        datasets: [{
                            data: values,
                            backgroundColor: color,
                            borderRadius: 6
                        }]
                    },
                    options: {
                        indexAxis: horizontal ? "y" : "x",
                        plugins: { legend: { display: false } },
                        scales: {
                            x: { ticks: { color: "#9a9a9a" }, grid: { color: "rgba(255,255,255,0.06)" }, beginAtZero: true },
                            y: { ticks: { color: "#9a9a9a" }, grid: { color: "rgba(255,255,255,0.06)" } }
                        }
                    }
                });
            }

            function rotateHero() {
                var hero = document.getElementById("dashboardHero");
                if (!hero) return;

                var layerA = hero.querySelector('.kc-dashboard-hero__bg--a');
                var layerB = hero.querySelector('.kc-dashboard-hero__bg--b');
                if (!layerA || !layerB) return;

                var posterRoot = hero.getAttribute('data-poster-root') || '/Content/images/movies/';
                if (posterRoot.charAt(posterRoot.length - 1) !== '/') posterRoot += '/';

                var posters = [
                    { file: 'Se7en.jpeg', position: 'center center' },
                    { file: 'IntoTheWild.jpeg', position: 'center center' },
                    { file: '3Idiots.jpeg', position: 'center top' },
                    { file: 'Endgame.jpeg', position: 'center center' },
                    { file: 'Interstellar.jpeg', position: 'center center' }
                ].map(function (poster) {
                    return {
                        url: posterRoot + poster.file,
                        position: poster.position
                    };
                });

                var activeLayer = layerA;
                var nextLayer = layerB;
                var index = 0;

                function preload(url, done) {
                    var img = new Image();
                    img.onload = function () { done(url); };
                    img.onerror = function () { done(null); };
                    img.src = url;
                }

                function setBackground(layer, poster) {
                    layer.style.backgroundImage = "url('" + String(poster.url).replace(/'/g, "%27") + "')";
                    layer.style.setProperty('--poster-position', poster.position || 'center center');
                }

                function showNextPoster() {
                    var attempts = 0;

                    function tryPoster() {
                        if (attempts >= posters.length) return;

                        var poster = posters[index];
                        index = (index + 1) % posters.length;
                        attempts++;

                        preload(poster.url, function (loadedUrl) {
                            if (!loadedUrl) {
                                tryPoster();
                                return;
                            }

                            setBackground(nextLayer, poster);
                            nextLayer.classList.add('is-active');
                            activeLayer.classList.remove('is-active');

                            var temp = activeLayer;
                            activeLayer = nextLayer;
                            nextLayer = temp;
                        });
                    }

                    tryPoster();
                }

                showNextPoster();
                setInterval(showNextPoster, 3600);
            }

            var bookings = parseField("hfChartBookings", { Labels: [], Values: [] });
            var tickets = parseField("hfChartTickets", { Labels: [], Values: [] });
            var occupancy = parseField("hfChartOccupancy", { Labels: [], Values: [] });

            renderLineChart("bookingsTrendChart", bookings, "Bookings for Shows");
            renderBarChart("ticketsByMovieChart", tickets, "#f5c518", false);
            renderBarChart("occupancyByTheaterChart", occupancy, "#e50914", true);
            rotateHero();
        })();
    </script>

</asp:Content>
