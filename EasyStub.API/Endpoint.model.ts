export interface UnsavedEndpoint {
  path: string;
  method: string;
  body: any;
  statusCode: number;
}

export type Endpoint =
  | UnsavedEndpoint
  | {
      id: string;
    };
