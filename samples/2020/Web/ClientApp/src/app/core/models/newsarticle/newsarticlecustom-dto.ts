export interface NewsArticleCustomDto {
    id: number;
    name: string;
    content: string;
    updatedBy: string;
    publishDate: Date;
    imageUrl: string;
    createdBy: string;
    isDeleted:boolean;
    canonicalUrl: string;
    status: string;
    publishImmediately: boolean;
   }
