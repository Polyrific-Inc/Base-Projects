import { EventDto } from "./event-dto";

export interface EventFilterResultDto {
  results: EventDto[];
  count: number;
}
