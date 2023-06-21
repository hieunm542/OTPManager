export interface SMSRequest {
    pageIndex: number;
    pageSize: number;
    from?: string;
    index?: number;
    content?: string;
    minReceivedTime?: Date;
    maxReceivedTime?: Date;
    phoneID?: string;
    phoneNumber?: string;
  }
  