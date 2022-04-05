"use strict";
//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md
Object.defineProperty(exports, "__esModule", { value: true });
exports.onDestroy = exports.onPublish = void 0;
const HotfixViewCodeGenerator_1 = require("./HotfixViewCodeGenerator");
function onPublish(handler) {
    if (!handler.genCode)
        return;
    handler.genCode = false; //prevent default output
    console.log('Handling gen code in plugin');
    HotfixViewCodeGenerator_1.hotfixViewCodeGenerator.Handle(handler);
    console.log('Handling gen code in plugin end');
}
exports.onPublish = onPublish;
function onDestroy() {
    //do cleanup here
}
exports.onDestroy = onDestroy;
