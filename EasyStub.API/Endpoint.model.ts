export type EndpointId = number;

export interface UnsavedEndpoint {
  path: string;
  method: string;
  body: any;
  statusCode: number;
}

export type Endpoint = UnsavedEndpoint & {
  id: EndpointId;
};

export type PatchEndpoint = {
  action: "EDIT" | "DELETE";
  endpoint: Endpoint;
};
