import { NotificationTypeDto } from "../notificationtype/notificationtype-dto";

export interface NotificationCustomDto {
    id: number;
    message: string;
    startDate: Date;
    endDate: Date;
    createdBy: string;
    updatedBy: string;
    notificationTypeId: number;
    notificationUrl: string;
    notificationRole: string;
    title: string;
    notificationTypeName: string;
    icon: string;
    highPriority: boolean;
    notificationType: NotificationTypeDto;
   }
