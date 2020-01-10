import { NotificationCustomDto } from "./notification-custom-dto";

export interface NotificationFilterResultDto {
    results: NotificationCustomDto[];
    count: number;
}
