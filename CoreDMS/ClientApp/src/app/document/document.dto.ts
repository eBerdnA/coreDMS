declare module DocumentDTO {

    export interface Tag {
        id: number;
        name: string;
        createdAt: string;
        updatedAt: string;
        fileTag: any[];
    }

    export interface FileTag {
        id: number;
        tagId: number;
        fileId: string;
        createdAt: string;
        updatedAt: string;
        tag: Tag;
    }

    export interface File {
        id: string;
        filename: string;
        hash: string;
        documentDate?: any;
        state: number;
        location: string;
        createdAt: string;
        updatedAt: string;
        title: string;
        note?: any;
        fileTag: FileTag[];
        documentFileFile?: any;
    }

    export interface FileState {
        disabled: boolean;
        group?: any;
        selected: boolean;
        text: string;
        value: string;
    }

    export interface RootObject {
        file: File;
        fileDate?: any;
        tags: string;
        fileStates: FileState[];
    }
}