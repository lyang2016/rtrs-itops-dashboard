$(function () {

    $(document).ready(function () {

        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        function requestData(currentTime, workFlowId) {
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

                    var chart;
                    switch (workFlowId) {
                        case 1:
                            chart = $('#container').highcharts();
                            break;
                        case 5:
                            chart = $('#container_1').highcharts();
                            break;
                    }
                    //var chart = $('#container').highcharts();
                    var series = chart.series[0];

                    var shift = series.data.length > nonShiftPoints; // shift if the series is longer than 20

                    //add the point
                    chart.series[0].addPoint([x, y], true, shift);


                    // call it again after one second
                    setTimeout(function () {
                        requestData(currentTime.add(pollingIntervalInSeconds, 'seconds'), workFlowId);
                    }, 5000);
                   
                },
                cache: false
            });
        }


        $('#container').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 1);
                    }
                }
            },
            title: {
                text: 'A-12 Getter'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150,
                minRange: pollingIntervalInSeconds * nonShiftPoints * 1000
            },
            yAxis: {
                title: {
                    text: 'Output'
                },
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
                name: 'A-12 Getter',
                data: []
            }]
        });

        $('#container_1').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container_1',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 5);
                    }
                }
            },
            title: {
                text: 'Parser'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150,
                minRange: pollingIntervalInSeconds * nonShiftPoints * 1000
            },
            yAxis: {
                title: {
                    text: 'Output'
                },
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
                name: 'Parser',
                data: []
            }]
        });

    });
});