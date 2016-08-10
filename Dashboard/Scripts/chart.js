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
                        case 0:
                            chart = $('#dqer').highcharts();
                            break;
                        case 1:
                            chart = $('#a12').highcharts();
                            break;
                        case 2:
                            chart = $('#csm').highcharts();
                            break;
                        case 3:
                            chart = $('#responder').highcharts();
                            break;
                        case 4:
                            chart = $('#processor').highcharts();
                            break;
                        case 5:
                            chart = $('#parser').highcharts();
                            break;
                        case 6:
                            chart = $('#matcher').highcharts();
                            break;
                        case 8:
                            chart = $('#disseminator').highcharts();
                            break;

                    }

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
                        requestData(currentTime.add(pollingIntervalInSeconds, 'seconds'), workFlowId);
                    }, pollingIntervalInSeconds * 1000);
                   
                },
                cache: false
            });
        }


        $('#dqer').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 0);
                    }
                }
            },
            title: {
                text: 'DQer'
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
                name: 'DQer',
                data: []
            }]
        });

        $('#parser').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 5);
                    }
                }
            },
            title: {
                text: 'Parser'
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
                name: 'Parser',
                data: []
            }]
        });

        $('#a12').highcharts({
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
                name: 'A-12 Getter',
                data: []
            }]
        });

        $('#csm').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 2);
                    }
                }
            },
            title: {
                text: 'CSM Getter'
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
                name: 'CSM Getter',
                data: []
            }]
        });

        $('#matcher').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container_1',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 6);
                    }
                }
            },
            title: {
                text: 'Matcher'
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
                name: 'Matcher',
                data: []
            }]
        });

        $('#processor').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container_1',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 4);
                    }
                }
            },
            title: {
                text: 'Processor'
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
                name: 'Processor',
                data: []
            }]
        });

        $('#responder').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container_1',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 3);
                    }
                }
            },
            title: {
                text: 'Responder'
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
                name: 'Responder',
                data: []
            }]
        });

        $('#disseminator').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                //renderTo: 'container_1',
                events: {
                    load: function () {
                        requestData(moment('2016-06-29 10:00:00'), 8);
                    }
                }
            },
            title: {
                text: 'Disseminator'
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
                name: 'Disseminator',
                data: []
            }]
        });

    });
});