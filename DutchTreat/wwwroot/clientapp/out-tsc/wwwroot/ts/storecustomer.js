"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var StoreCustomer = /** @class */ (function () {
    function StoreCustomer(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.visits = 0;
    }
    StoreCustomer.prototype.showName = function () {
        alert(this.firstName + " " + this.lastName);
        return true;
    };
    Object.defineProperty(StoreCustomer.prototype, "name", {
        get: function () {
            return this.ourName;
        },
        set: function (value) {
            this.ourName = value;
        },
        enumerable: true,
        configurable: true
    });
    return StoreCustomer;
}());
exports.StoreCustomer = StoreCustomer;
//# sourceMappingURL=storecustomer.js.map