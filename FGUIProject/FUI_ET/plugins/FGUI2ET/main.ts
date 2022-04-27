//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md

import { FairyEditor } from 'csharp';
import { fuiCodeGenerator } from './FUICodeGenerator';

function onPublish(handler: FairyEditor.PublishHandler) {
    if (!handler.genCode) return;
    handler.genCode = false; //prevent default output

    console.log('Handling gen code in plugin');
    fuiCodeGenerator.Handle(handler);
    console.log('Handling gen code in plugin end');
}

function onDestroy() {
    //do cleanup here
}

export { onPublish, onDestroy };