import { NewsArticleDto } from './newsarticle-dto';

export interface NewsArticleFilterResultDto {
  results: NewsArticleDto[];
  count: number;
}
