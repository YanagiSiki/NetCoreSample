declare interface JQuery {
    typeahead(option: any, value: any): JQuery;
    serializeJSON(): JQuery;
    dropdown(option?: any): JQuery;
    calendar(option1?: any, option2?: any): JQuery;
}
// SimpleMDE 型別請只保留一份宣告，若有外部型別請移除本地宣告。
declare class SimpleMDE {
    constructor(option: any);
    value(): string;
    toTextArea(): void;
    value(value: string): void;
    togglePreview(): void;
    markdown(value: string): string;
}

declare var anchors: any;
declare interface Tag {
    TagId: number;
    TagName: string;
}
declare interface PostCountOfTag {
    Tag: Tag;
    Count: number;
}
declare namespace hljs {
    function initHighlightingOnLoad(): void;
    function initLineNumbersOnLoad(): void;
}