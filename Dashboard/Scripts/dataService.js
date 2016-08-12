var dataService = function() {

    var getChartData = function(apiUrl) {
        return $.ajax({
            type: 'GET',
            url: apiUrl,
            dataType: 'jsonp',
            crossDomain: true,
            cache: false
        });
    };

    var getSite1ChartData = function (apiUrl) {
        return getChartData(apiUrl);
    };

    var getSite2ChartData = function (apiUrl) {
        return getChartData(apiUrl);
    };

    var getAllSitesChartData = function(url1, url2) {
        var promises = [getSite1ChartData(url1), getSite2ChartData(url2)];
        return $.when.apply($, promises);
    };

    return {
        getChartData: getChartData,
        getAllSitesChartData: getAllSitesChartData
    };
}();