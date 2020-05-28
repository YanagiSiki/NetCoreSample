declare interface JQuery {
    typeahead(option: any, value: any): JQuery;
    serializeJSON(): JQuery;
    dropdown(option?: any): JQuery;
    calendar(option1?: any, option2?: any): JQuery;
}
declare class SimpleMDE {
    constructor(option: any);
    value(): string;
    toTextArea(): void;
    value(value: string): void;
    togglePreview(): void;
    markdown(value: string): string;
}
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