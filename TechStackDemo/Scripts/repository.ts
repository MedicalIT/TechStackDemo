//SignalR
interface SignalR {
    counterHub: {
        server: {
            getCounterItem: (id: string) => JQueryPromise<Data.CounterItem>;
            increment: (id: string, qty: number) => JQueryPromise<Data.CounterItem>,
            decrement: (id: string, qty: number) => JQueryPromise<Data.CounterItem>,
            reset: () => JQueryPromise<void>
        },
        client: {
            allCountersReset: () => void;
            newCounterValues: (values: Data.CounterItem[]) => void;
            newCounterValue: (value: Data.CounterItem) => void;
        }
    }
}


namespace Data {

    export interface CounterItem {
        Id: string;
        Value: number;
    }
    
    
    class CounterRepository {

        constructor() {

        }
        
        public hubClient = {

            allCountersReset: () => {
                this.dict.forEach((key, ob) => ob({ Id: key, Value: undefined }));
            },

            newCounterValues: (values: Data.CounterItem[]) => {
                values.forEach(value=> this.ensureInDict(value.Id)(value));
            }
        };
        

        private dict = new collections.ObservableDictionary<string, KnockoutObservable<CounterItem>>(s=> s.toLowerCase());

        private ensureInDict = (id: string) => this.dict.getOrAddValue(id, () => ko.observable({ Id: id, Value: undefined }));

        private subscribed = new collections.Set<string>(s=> s.toLowerCase());

        public getCounter = (id: string) => {
            var result = this.ensureInDict(id);

            if (this.subscribed.add(id)) {
                //get initial value...
                Data.hubStart().then(() => $.connection.counterHub.server.getCounterItem(id)).then(
                    ctr=> this.dict.getOrAddValue(id,
                        () => ko.observable(ctr),
                        existingCtrObservable=> existingCtrObservable(ctr)
                    )
                );
            }

            return result;
        }

        public increment = (id: string, qty: number = 1) => {
            Data.hubStart().then(() => $.connection.counterHub.server.increment(id, qty));  //throw away the result: we'll just wait for notification
        }



        public getCounterIds = ko.pureComputed(() => this.dict.observableKeys());
        

        public reset = () => 
            Data.hubStart().then(() => $.connection.counterHub.server.reset()).then(() => this.hubClient.allCountersReset());
        
    }

    export var counterRepository = new CounterRepository();

    //wire up listener for the counter repository
    //$.connection.counterHub.client = _.extend($.connection.counterHub.client, counterRepository.hubClient);
    $.connection.counterHub.client.allCountersReset = () => counterRepository.hubClient.allCountersReset();
    $.connection.counterHub.client.newCounterValues = (values: CounterItem[]) => counterRepository.hubClient.newCounterValues(values);
    $.connection.counterHub.client.newCounterValue = (value: CounterItem) => counterRepository.hubClient.newCounterValues([value]);

    //(<any>$.connection.counterHub).on('newCounterValue', (value) => counterRepository.hubClient.newCounterValues([value]));

    var started: JQueryPromise<void>;
    export function hubStart(onStart?: () => void) {        
        if (!started) {
            started = $.connection.hub.start();
        }
        
        if (onStart) {
            return started.then(() => onStart());
        }
        return started;
    }
}