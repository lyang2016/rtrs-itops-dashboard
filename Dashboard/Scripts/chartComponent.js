<<<<<<< HEAD
﻿function requestData(thisObj, currentTime, workFlowId, yAxisFrom, yAxisTo, yAxisFixedFlag) {
    var to = moment(currentTime);
    var from = to.clone().subtract(pollingIntervalInSeconds, 'seconds');

    var arrCenters = $.parseJSON(dataCenters.replace(new RegExp('&quot;', 'g'), '"'));

    //for (var index in arrCenters) {
    //    var apiUrl = arrCenters[index].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss');
    //    console.log(apiUrl);
    //}

    var site1_url = arrCenters[0].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss') + '&workflowId=' + workFlowId;
    var site2_url = arrCenters[1].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss') + '&workflowId=' + workFlowId;
    var site3_url = arrCenters[2].WebApiUrl + '?from=' + from.format('YYYY-MM-DD HH:mm:ss') + '&to=' + to.format('YYYY-MM-DD HH:mm:ss') + '&workflowId=' + workFlowId;

    MSRB.dataService.getAllSitesChartData(site1_url, site2_url, site3_url)
            .done(function (data1, data2, data3) {
                var chart = thisObj;

                // set fixed or auto scaled y-axis
                if (yAxisFixedFlag === 'true') {
                    chart.yAxis[0].setExtremes(yAxisFrom, yAxisTo);
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
                chart.series[1].addPoint([x, y], false, shift);

                // series 3
                series = chart.series[2];
                shift = series.data.length > nonShiftPoints; // shift if the series is longer than 20

                arr = data3[0];
                if (arr.length > 0) {
                    // Replace T with space to prevent it to be treated as UTC date string
                    x = Date.parse(arr[0].CompletionSecond.replace('T', ' '));
                    y = arr[0].MessageCount;
                } else {
                    x = Date.parse(currentTime.format('YYYY-MM-DD HH:mm:ss'));
                    y = 0;
                }

                //add the point
                chart.series[2].addPoint([x, y], true, shift);


                // call it again after polling interval in seconds
                setTimeout(function () {
                    requestData(chart, currentTime.add(pollingIntervalInSeconds, 'seconds'), workFlowId, yAxisFrom, yAxisTo, yAxisFixedFlag);
                }, pollingIntervalInSeconds * 1000);
            })
            .fail(function (jqXHR, statusText, err) {
                var errorMessage = err || xhr.statusText;
                alert(errorMessage);
            });

}



MSRB.Chart = function (name, container, pollingIntervalInSeconds, nonShiftPoints, workFlowId, yAxisFrom, yAxisTo, yAxisFixedFlag, yAxisTitleText, timeLaggingToPull, arrCenters) {
=======
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
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    this.chart = new Highcharts.Chart({
        chart: {
            type: 'spline',
            animation: Highcharts.svg, // don't animate in old IE
            marginRight: 10,
            renderTo: container,
<<<<<<< HEAD
            height: 250,
            events: {
                load: function () {
                    var currentTime;
                    if (playbackEnabled === 'true') {
                        currentTime = moment(playbackStartTime);
                    } else {
                        currentTime = moment(moment().add(timeLaggingToPull * -1, 'seconds').format('YYYY-MM-DD HH:mm:ss'));
                    }
                    requestData(this, currentTime, workFlowId, yAxisFrom, yAxisTo, yAxisFixedFlag);
=======
            events: {
                load: function () {
                    requestData(this, moment('2016-06-29 10:00:00'), workFlowId);
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
                }
            }
        },
        title: {
<<<<<<< HEAD
            text: name,
            style: {
                color: '#003366',
                fontWeight: 'bold'
            }
=======
            text: name
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'datetime',
            tickPixelInterval: 150,
<<<<<<< HEAD
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
                text: yAxisTitleText
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
=======
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
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
        legend: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        series: [{
            name: name,
<<<<<<< HEAD
            data: [],
            color: arrCenters[0].LineColor
        },
        {
            name: name,
            data: [],
            color: arrCenters[1].LineColor
        },
        {
            name: name,
            data: [],
            color: arrCenters[2].LineColor
=======
            data: []
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
        }]
    });


};

