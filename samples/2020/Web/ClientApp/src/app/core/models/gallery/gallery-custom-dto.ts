import { PhotoDto } from "../photo/photo-dto";

export interface GalleryCustomDto {
    id: number;
    name: string;
    description: string;
    coverImageUrl: string;
    createdById: number;
    guid: string;    
    status: string;
    photos: PhotoDto[];
    sendNotification: boolean;
    featuredAlbums:boolean;
    publishDate: Date;
   }
