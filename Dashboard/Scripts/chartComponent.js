﻿function requestData(thisObj, currentTime, workFlowId) {
    var to = moment(currentTime);
    var from = to.clone().subtract(pollingIntervalInSeconds, 'seconds');
    var apiUrl = chartUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss');
    console.log(apiUrl);
    $.ajax({
        type: 'GET',
        url: apiUrl,
        dataType: 'json',
        success: function (data) {
            var arr, x, y;
            arr = $.parseJSON(data.Result);
            if (arr.length > 0) {
                // Replace T with space to prevent it to be treated as UTC date string
                x = Date.parse(arr[0].CompletionSecond.replace('T', ' '));
                y = 0;
                for (var i in arr) {
                    var id = arr[i].WorkflowId;
                    if (id == workFlowId) {
                        y = arr[i].MessageCount;
                        break;
                    }
                }
            } else {
                x = Date.parse(currentTime.format('YYYY-MM-DD HH:mm:ss'));
                y = 0;
            }

            var chart = thisObj;

            // set fixed or auto scaled y-axis
            if (yAxisFixed === 'true') {
                chart.yAxis[0].setExtremes(yAxisMin, yAxisMax);
            }

            var series = chart.series[0];

            var shift = series.data.length > nonShiftPoints; // shift if the series is longer than 20

            //add the point
            chart.series[0].addPoint([x, y], true, shift);


            // call it again after polling interval in seconds
            setTimeout(function () {
                requestData(chart, currentTime.add(pollingIntervalInSeconds, 'seconds'), workFlowId);
            }, pollingIntervalInSeconds * 1000);

        },
        cache: false
    });
}

// define MSRB namespace
var MSRB = MSRB || {};

MSRB.Chart = function (name, container, pollingIntervalInSeconds, nonShiftPoints, workFlowId) {
    this.chart = new Highcharts.Chart({
        chart: {
            type: 'spline',
            animation: Highcharts.svg, // don't animate in old IE
            marginRight: 10,
            renderTo: container,
            events: {
                load: function () {
                    requestData(this, moment('2016-06-29 10:00:00'), workFlowId);
                }
            }
        },
        title: {
            text: name
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150,
            minRange: pollingIntervalInSeconds * nonShiftPoints * 1000
        },
        yAxis: {
            title: {
                text: ''
            },
            //max: 1000,
            //min: 0,
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        legend: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        series: [{
            name: name,
            data: []
        }]
    });


};
