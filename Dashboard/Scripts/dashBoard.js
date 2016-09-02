
$(function () {

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    var dqer = new MSRB.Chart('DQer', 'dqer', pollingIntervalInSeconds, nonShiftPoints, 0, yAxisMin, yAxisMax, yAxisFixed);

    var parser = new MSRB.Chart('Parser', 'parser', pollingIntervalInSeconds, nonShiftPoints, 5, yAxisMin, yAxisMax, yAxisFixed);

    var a12 = new MSRB.Chart('A-12 Getter', 'a12', pollingIntervalInSeconds, nonShiftPoints, 1, yAxisMin, yAxisMax, yAxisFixed);

    var csm = new MSRB.Chart('CSM Getter', 'csm', pollingIntervalInSeconds, nonShiftPoints, 2, yAxisMin, yAxisMax, yAxisFixed);

    var matcher = new MSRB.Chart('Matcher', 'matcher', pollingIntervalInSeconds, nonShiftPoints, 6, yAxisMin, yAxisMax, yAxisFixed);

    var processor = new MSRB.Chart('Processor', 'processor', pollingIntervalInSeconds, nonShiftPoints, 4, yAxisMin, yAxisMax, yAxisFixed);

    var responder = new MSRB.Chart('Responder', 'responder', pollingIntervalInSeconds, nonShiftPoints, 3, yAxisMin, yAxisMax, yAxisFixed);

    var disseminator = new MSRB.Chart('Disseminator', 'disseminator', pollingIntervalInSeconds, nonShiftPoints, 8, yAxisMin, yAxisMax, yAxisFixed);

    var ete = new MSRB.Chart('End-to-End Time to Response', 'ete', pollingIntervalInSeconds, nonShiftPoints, 100, yAxisMinEte, yAxisMaxEte, yAxisFixedEte);

    var inbound = new MSRB.Chart('Inbound Queue', 'inbound', pollingIntervalInSeconds, nonShiftPoints, 101, yAxisMinMq, yAxisMaxMq, yAxisFixedMq);

    var outbound = new MSRB.Chart('Outbound Queue', 'outbound', pollingIntervalInSeconds, nonShiftPoints, 102, yAxisMinMq, yAxisMaxMq, yAxisFixedMq);

});
