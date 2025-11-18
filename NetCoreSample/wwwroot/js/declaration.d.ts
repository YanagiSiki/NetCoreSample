import 'jquery';

declare global {
    interface JQuery {
        typeahead(option: any, value: any): JQuery;
        serializeJSON(): JQuery;
        dropdown(option?: any): JQuery;
        calendar(option1?: any, option2?: any): JQuery;
    }
    declare var anchors: any;
    class SimpleMDE {
        constructor(option: any);
        value(): string;
        toTextArea(): void;
        value(value: string): void;
        togglePreview(): void;
        markdown(value: string): string;
    }
}


declare interface Tag {
    TagId: number;
    TagName: string;
}
declare interface Tags {
    Tag: Tag;
    Count: number;
}
declare namespace hljs {
    function initHighlightingOnLoad(): void;
    function initLineNumbersOnLoad(): void;
}

// SimpleMDE module 型別宣告
// declare module 'simplemde' {
//     export default class SimpleMDE {
//         constructor(option: any);
//         value(): string;
//         toTextArea(): void;
//         value(value: string): void;
//         togglePreview(): void;
//         markdown(value: string): string;
//     }
// }