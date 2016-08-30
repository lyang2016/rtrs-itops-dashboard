﻿function requestData(thisObj, currentTime, workFlowId) {
    var to = moment(currentTime);
    var from = to.clone().subtract(pollingIntervalInSeconds, 'seconds');

    var arrCenters = $.parseJSON(dataCenters.replace(new RegExp('&quot;', 'g'), '"'));

    //for (var index in arrCenters) {
    //    var apiUrl = arrCenters[index].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss');
    //    console.log(apiUrl);
    //}

    var site1_url = arrCenters[0].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss') + '&workflowId=' + workFlowId;
    var site2_url = arrCenters[1].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss') + '&workflowId=' + workFlowId;

    MSRB.dataService.getAllSitesChartData(site1_url, site2_url)
            .done(function (data1, data2) {
                var chart = thisObj;

                // set fixed or auto scaled y-axis
                if (yAxisFixed === 'true') {
                    chart.yAxis[0].setExtremes(yAxisMin, yAxisMax);
                }

                var arr, x, y;
                // series 1
                var series = chart.series[0];
                var shift = series.data.length > nonShiftPoints; // shift if the series is longer than 20

                arr = data1[0];
                if (arr.length > 0) {
                    // Replace T with space to prevent it to be treated as UTC date string
                    x = Date.parse(arr[0].CompletionSecond.replace('T', ' '));
                    y = arr[0].MessageCount;
                } else {
                    x = Date.parse(currentTime.format('YYYY-MM-DD HH:mm:ss'));
                    y = 0;
                }

                //add the point
                chart.series[0].addPoint([x, y], false, shift);

                // series 2
                series = chart.series[1];
                shift = series.data.length > nonShiftPoints; // shift if the series is longer than 20

                arr = data2[0];
                if (arr.length > 0) {
                    // Replace T with space to prevent it to be treated as UTC date string
                    x = Date.parse(arr[0].CompletionSecond.replace('T', ' '));
                    y = arr[0].MessageCount;
                } else {
                    x = Date.parse(currentTime.format('YYYY-MM-DD HH:mm:ss'));
                    y = 0;
                }

                //add the point
                chart.series[1].addPoint([x, y], true, shift);


                // call it again after polling interval in seconds
                setTimeout(function () {
                    requestData(chart, currentTime.add(pollingIntervalInSeconds, 'seconds'), workFlowId);
                }, pollingIntervalInSeconds * 1000);
            })
            .fail(function (jqXHR, statusText, err) {
                var errorMessage = err || xhr.statusText;
                alert(errorMessage);
            });

}



MSRB.Chart = function (name, container, pollingIntervalInSeconds, nonShiftPoints, workFlowId) {
    this.chart = new Highcharts.Chart({
        chart: {
            type: 'spline',
            animation: Highcharts.svg, // don't animate in old IE
            marginRight: 10,
            renderTo: container,
            height: 250,
            events: {
                load: function() {
                    requestData(this, moment('2016-07-28 11:30:00'), workFlowId);
                }
            }
        },
        title: {
            text: name,
            style: {
                color: '#003366',
                fontWeight: 'bold'
            }
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150,
            minRange: pollingIntervalInSeconds * nonShiftPoints * 1000,
            labels: {
                style: {
                    color: '#333',
                    fontWeight:'bold'
                }
            }
        },
        yAxis: {
            title: {
                text: ''
            },
            //max: 1000,
            //min: 0,
            labels: {
                style: {
                    color: '#333',
                    fontWeight: 'bold'
                }
            }
            //plotLines: [{
            //    value: 0,
            //    width: 1,
            //    color: '#808080'
            //}]
        },
        //plotOptions:{
        //    series:{
        //        lineWidth: 5,
        //        color: '#ee7406'
        //    }
        //},
        legend: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        series: [{
            name: name,
            data: [],
            color: '#003874'
        },
        {
            name: 'Fake',
            data: [],
            color: '#2c992e'
        }]
    });


};

