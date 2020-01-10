import { GalleryCustomDto } from './gallery-custom-dto';

export interface GalleryFilterResultDto {
  results: GalleryCustomDto[];
  count: number;
}
