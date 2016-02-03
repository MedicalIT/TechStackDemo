//Easier way to access common extenders (TypeScript definitions created to match these functions)
//RateLimit: notify when changes stop
ko.subscribable.fn.rateLimitNotifyWhenStop = function (timeout) {
    return this.extend({ rateLimit: { timeout: timeout, method: "notifyWhenChangesStop" } });
};
//RateLimit: notify at a fixed rate
ko.subscribable.fn.rateLimitFixedRate = function (timeout) {
    return this.extend({ rateLimit: { timeout: timeout, method: "notifyAtFixedRate" } });
};
//# sourceMappingURL=knockoutExtenders.js.map