
$(function () {

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    var arrCenters = $.parseJSON(dataCenters.replace(new RegExp('&quot;', 'g'), '"'));

    var dqer = new MSRB.Chart('DQer', 'dqer', pollingIntervalInSeconds, nonShiftPoints, 0, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var parser = new MSRB.Chart('Parser', 'parser', pollingIntervalInSeconds, nonShiftPoints, 5, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var a12 = new MSRB.Chart('A-12 Getter', 'a12', pollingIntervalInSeconds, nonShiftPoints, 1, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var csm = new MSRB.Chart('CSM Getter', 'csm', pollingIntervalInSeconds, nonShiftPoints, 2, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var matcher = new MSRB.Chart('Matcher', 'matcher', pollingIntervalInSeconds, nonShiftPoints, 6, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var rulesengine = new MSRB.Chart('Rules Engine', 'rulesengine', pollingIntervalInSeconds, nonShiftPoints, 4, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var responder = new MSRB.Chart('Responder', 'responder', pollingIntervalInSeconds, nonShiftPoints, 3, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var disseminator = new MSRB.Chart('Disseminator', 'disseminator', pollingIntervalInSeconds, nonShiftPoints, 8, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var rsequencer = new MSRB.Chart('Responder Sequencer', 'rsequencer', pollingIntervalInSeconds, nonShiftPoints, 7, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var dsequencer = new MSRB.Chart('Disseminator Sequencer', 'dsequencer', pollingIntervalInSeconds, nonShiftPoints, 9, yAxisMin, yAxisMax, yAxisFixed, yAxisTitleText, timeLaggingToPull, arrCenters);

    var ete = new MSRB.Chart('End-to-End Time to Response', 'ete', pollingIntervalInSeconds, nonShiftPoints, 100, yAxisMinEte, yAxisMaxEte, yAxisFixedEte, yAxisTitleTextEte, timeLaggingToPull, arrCenters);

    var inbound = new MSRB.Chart('Inbound Queue', 'inbound', pollingIntervalInSeconds, nonShiftPoints, 101, yAxisMinMq, yAxisMaxMq, yAxisFixedMq, yAxisTitleTextMq, timeLaggingToPull, arrCenters);

    var outbound = new MSRB.Chart('Outbound Queue', 'outbound', pollingIntervalInSeconds, nonShiftPoints, 102, yAxisMinMq, yAxisMaxMq, yAxisFixedMq, yAxisTitleTextMq, timeLaggingToPull, arrCenters);

});
