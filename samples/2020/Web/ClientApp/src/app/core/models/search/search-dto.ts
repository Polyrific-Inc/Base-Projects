export interface SearchDto {
    Events:EventSearchDto;
    Users:UserSearchDto;
    News:NewsArticleSearchDTO;
    Document:DocumentSearchDTO;
    DocumentGroup: DocumentGroupSearchDTO;
    DocumentCategory: DocumentCategorySearchDTO;
}

export interface EventSearchDto{
    items:itemsSearchDto[];
}

export interface UserSearchDto{
    items:itemsSearchDto[];
}

export interface NewsArticleSearchDTO{
    items:itemsSearchDto[];
}

export interface DocumentGroupSearchDTO{
    items:itemsSearchDto[];
}

export interface DocumentCategorySearchDTO{
    items:itemsSearchDto[];
}

export interface DocumentSearchDTO{
    items:itemsSearchDto[];
}

export interface itemsSearchDto{
    title:string;
    content:string;
}

