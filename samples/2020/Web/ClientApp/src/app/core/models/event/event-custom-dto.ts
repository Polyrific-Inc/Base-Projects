
export interface EventCustomDto {
    id: number;
    name: string;
    description: string;
    eventStartDate: Date;
    address: string;
    eventAccessLevelId: number;
    maxParticipant: number;
    updatedBy: string;
    createdBy: string;
    imageUrl: string;
    eventEndDate: Date;
    eventReservationsIds: number[];
    canonicalUrl: string;
    status: string;
    publishDate: Date;
    publishImmediately: boolean;
    sendNotification: boolean;
    eventStartDateString: string;
   }
   
   