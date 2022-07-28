mapboxgl.accessToken = "pk.eyJ1IjoiaG9hbmMxOTgxIiwiYSI6ImNram95YW9uYzB0M24yd3BlbXVkOWNmN3QifQ.Z681DzM2rasHXmZKEcyYbQ";
var geojson =  @Html.Raw(ViewData["lstGeoClearance"]);
 
var map = new mapboxgl.Map({
    container: 'map', 
style: 'mapbox://styles/mapbox/satellite-streets-v11',
    center: [106.625344, 17.467754],
    zoom: 8.2
});

this.map.on('load', function (e) {
     

    e.target.addSource('earthquakes', {
        'type': 'geojson',
        'data': 
    });


    e.target.addLayer(
        {
            'id': 'earthquakes-heat',
            'type': 'heatmap',
            'source': 'earthquakes',
            'maxzoom': 15,
            'paint': {
            
                'heatmap-weight': [
                    'interpolate',
                    ['linear'],
                    ['get', 'mag'],
                    0,
                    0,
                    2,
                    1
                ],
          
                'heatmap-intensity': [
                    'interpolate',
                    ['linear'],
                    ['zoom'],
                    0,
                    1,
                    9,
                    3
                ],
          
                'heatmap-color': [
                    'interpolate',
                    ['linear'],
                    ['heatmap-density'],
                    0,
                    'rgba(33,102,172,0)',
                    0.2,
                    'rgb(103,169,207)',
                    0.4,
                    'rgb(209,229,240)',
                    0.6,
                    'rgb(253,219,199)',
                    0.8,
                    'rgb(239,138,98)',
                    1,
                    'rgb(178,24,43)'
                ],
               
                'heatmap-radius': [
                    'interpolate',
                    ['linear'],
                    ['zoom'],
                    0,
                    2,
                    12,
                    15
                ],
             
                'heatmap-opacity': [
                    'interpolate',
                    ['linear'],
                    ['zoom'],
                    9,
                    1,
                    19,
                    0
                ]
            }
        },
        'waterway-label'
    );

    e.target.addLayer(
        {
            'id': 'earthquakes-point',
            'type': 'circle',
            'source': 'earthquakes',
            'minzoom': 7,
            'paint': {
            
                'circle-radius': [
                    'interpolate',
                    ['linear'],
                    ['zoom'],
                    12,
                    ['interpolate', ['linear'], ['get', 'mag'], 1, 1, 6, 4],
                    16,
                    ['interpolate', ['linear'], ['get', 'mag'], 1, 5, 6, 50]
                ],
            
                'circle-color': [
                    'interpolate',
                    ['linear'],
                    ['get', 'mag'],
                    1,
                    'rgba(33,102,172,0)',
                    2,
                    'rgb(103,169,207)',
                    3,
                    'rgb(209,229,240)',
                    4,
                    'rgb(253,219,199)',
                    5,
                    'rgb(239,138,98)',
                    6,
                    'rgb(178,24,43)'
                ],
                'circle-stroke-color': 'blue',
                'circle-stroke-width': 1,
              
                'circle-opacity': [
                    'interpolate',
                    ['linear'],
                    ['zoom'],
                    7,
                    1,
                    8,
                    1
                ]
            }
        },
        'waterway-label'
    );
});