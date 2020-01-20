let map = {};

var popup = L.popup().setContent('<div>Add Marker?</div><button onclick="AddIconToMap()">Yes</button><button onclick="ClosePopups()">No</button>');

window.leafletBlazor = {

    createImageMap: function (mapId, img, w, h) {

        if (map !== undefined) {
            try {
                map.off();
                map.remove();
            } catch(ex) {
                console.log("can't remove map");
            }
        }
        map = L.map(mapId,
            {
                minZoom: 0,
                maxZoom: 6,
                center: [0, 0],
                zoom: 0,
                crs: L.CRS.Simple,
                attributionControl: false
            });

        var southWest = map.unproject([0, h], map.getMaxZoom() - 5);
        var northEast = map.unproject([w, 0], map.getMaxZoom() - 5);
        var bounds = new L.latLngBounds(southWest, northEast);

        L.imageOverlay(img, bounds).addTo(map);

        map.setMaxBounds(bounds);

        map.on('contextmenu', (e) => {
            popup.setLatLng(e.latlng)
                .openOn(map);
        });

    },

    addMarker: function(lat, lng, content) {
        var marker = L.marker([lat, lng]).addTo(map);
        marker.bindPopup(content);
    }
};

function AddIconToMap() {
    DotNet.invokeMethodAsync("Mytheme.Map", "AddMarkerInterop", popup._latlng.lat, popup._latlng.lng);
    map.closePopup();
}

function ClosePopups() {
    map.closePopup();
}