
$(function () {

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    var dqer = new MSRB.Chart('DQer', 'dqer', pollingIntervalInSeconds, nonShiftPoints, 0);

    var parser = new MSRB.Chart('Parser', 'parser', pollingIntervalInSeconds, nonShiftPoints, 5);

    var a12 = new MSRB.Chart('A-12 Getter', 'a12', pollingIntervalInSeconds, nonShiftPoints, 1);

    var csm = new MSRB.Chart('CSM Getter', 'csm', pollingIntervalInSeconds, nonShiftPoints, 2);

    var matcher = new MSRB.Chart('Matcher', 'matcher', pollingIntervalInSeconds, nonShiftPoints, 6);

    var processor = new MSRB.Chart('Processor', 'processor', pollingIntervalInSeconds, nonShiftPoints, 4);

    var responder = new MSRB.Chart('Responder', 'responder', pollingIntervalInSeconds, nonShiftPoints, 3);

    var disseminator = new MSRB.Chart('Disseminator', 'disseminator', pollingIntervalInSeconds, nonShiftPoints, 8);

});
